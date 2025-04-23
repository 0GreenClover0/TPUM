using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelMainWindow : INotifyPropertyChanged
    {
        private static readonly string noneParty = "None";
        private readonly AbstractModelAPI modelAPI;
        private IModelCandidate selectedCandidate;
        private string selectedParty = noneParty;
        private int timeLeft = 20;

        public ObservableCollection<string> CandidateParties { get; set; }
        public ObservableCollection<CandidateAndInfo> CandidatesAndInfo { get; set; }
        public ObservableCollection<IModelCandidate> ModelCandidates { get; set; }
        public List<CandidateAndInfo> CandidatesAndInfoCache = new List<CandidateAndInfo>();
        public ICommand SelectCandidateCommand { get; }
        public ICommand MoreInfoCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnCandidatesRefreshed()
        {
        }

        public IModelCandidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set
            {
                selectedCandidate = value;
                NotifyPropertyChanged();
            }
        }

        public string SelectedParty
        {
            get { return selectedParty; }
            set
            {
                if (value == null)
                    return;

                FilterCandidates(value);
                selectedParty = value;
                NotifyPropertyChanged();
            }
        }

        public int TimeLeft
        {
            get { return timeLeft; }
            set
            {
                timeLeft = value;
                NotifyPropertyChanged();
            }
        }

        private async void CandidatesRefreshCountdown()
        {
            while (true)
            {
                LoadCandidates();
                LoadCandidatesAndInfo();
                FilterCandidates(selectedParty);
                await Task.Delay(1000);
            }
        }

        public ViewModelMainWindow()
        {
            modelAPI = AbstractModelAPI.CreateNewInstance();
            ModelCandidates = [];
            CandidatesAndInfo = [];
            CandidateParties = [];
            modelAPI.TimerUpdated += OnTimerUpdated;
            modelAPI.CandidateInfoUpdated += OnCandidateInfoUpdated;
            modelAPI.CandidatesRefreshed += OnCandidatesRefreshed;
            modelAPI.GetConnection().OnConnectionStateChanged += OnConnectionStateChanged;

            CandidatesRefreshCountdown();
            OnConnectionStateChanged();

            SelectCandidateCommand = new RelayCommand<CandidateAndInfo>(candidate =>
            {
                if (candidate != null)
                {
                    foreach (var c in ModelCandidates)
                        modelAPI.DeselectCandidate(c.ID);  // Deselect others

                    modelAPI.ChooseCandidate(candidate.Candidate.ID);
                    SelectedCandidate = candidate.Candidate;
                }

                OnConnectionStateChanged();
            });

            MoreInfoCommand = new RelayCommand<CandidateAndInfo>(candidate =>
            {
                if (candidate == null)
                    return;

                OnMoreInfoCandidate(candidate.Candidate.ID);
            });
        }

        private void OnConnectionStateChanged()
        {
            bool actualState = modelAPI.GetConnection().IsConnected();
            string connectionStatus = actualState ? "Connected" : "Disconnected";

            if (!actualState)
            {
                Task.Run(() => modelAPI.GetConnection().Connect(new Uri(@"ws://localhost:42069")));
            }
            else
            {
                modelAPI.SendChooseCandidate();
            }
        }

        private void OnMoreInfoCandidate(int id)
        {
            modelAPI.MoreInfoCandidate(id);
        }

        private void OnTimerUpdated(int newTime)
        {
            TimeLeft = newTime;
        }

        private void OnCandidateInfoUpdated(string newInfo, int ID)
        {
            LoadCandidateInfo(newInfo, ID);
        }

        private void FilterCandidates(string party)
        {
            bool everyone = party == noneParty;

            CandidatesAndInfo.Clear();

            foreach (var candidate in ModelCandidates)
            {
                if (everyone || candidate.Party == party)
                {
                    var c = new CandidateAndInfo(candidate);
                    CandidateAndInfo cache = CandidatesAndInfoCache.FirstOrDefault(x => x.Candidate.ID == candidate.ID);

                    if (cache != null)
                    {
                        c.Info = cache.Info;
                    }

                    CandidatesAndInfo.Add(c);
                }
            }

            NotifyPropertyChanged();
        }

        private void LoadCandidates()
        {
            var candidates = modelAPI.GetModelCandidates();

            if (!CandidateParties.Contains(noneParty))
                CandidateParties.Add(noneParty);

            ModelCandidates.Clear();
            foreach (var candidate in candidates)
            {
                ModelCandidates.Add(candidate);

                if (!CandidateParties.Contains(candidate.Party))
                {
                    CandidateParties.Add(candidate.Party);
                }
            }

            // modelAPI.RefreshModel();
            NotifyPropertyChanged();
        }

        private void LoadCandidatesAndInfo()
        {
            CandidatesAndInfoCache = CandidatesAndInfo.ToList();
            CandidatesAndInfo.Clear();
            foreach (var candidate in ModelCandidates)
            {
                CandidateAndInfo c = new CandidateAndInfo(candidate);
                CandidateAndInfo cache = CandidatesAndInfoCache.FirstOrDefault(x => x.Candidate.ID == candidate.ID);
                if (cache != null)
                {
                    c.Info = cache.Info;
                }
                CandidatesAndInfo.Add(c);
            }
        }

        private void LoadCandidateInfo(string newInfo, int ID)
        {
            var candidate = CandidatesAndInfo.FirstOrDefault(x => x.Candidate.ID == ID);
            if (candidate != null)
            {
                candidate.Info = newInfo;
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CandidateAndInfo : INotifyPropertyChanged
    {
        public IModelCandidate Candidate { get; set; }
        public string Info
        {
            get => info;
            set
            {
                if (info != value)
                {
                    info = value;
                    OnPropertyChanged();
                }
            }
        }

        private string info;

        public CandidateAndInfo(IModelCandidate candidate)
        {
            Candidate = candidate;
            Info = "";
        }

        public CandidateAndInfo(IModelCandidate candidate, string info)
        {
            Candidate = candidate;
            Info = info;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}