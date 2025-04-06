using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelMainWindow : INotifyPropertyChanged
    {
        private readonly AbstractModelAPI modelAPI;
        private IModelCandidate selectedCandidate;
        private int timeLeft = 20;

        public ObservableCollection<IModelCandidate> ModelCandidates { get; set; }
        public ICommand SelectCandidateCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public IModelCandidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set
            {
                selectedCandidate = value;
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

        public ViewModelMainWindow()
        {
            modelAPI = AbstractModelAPI.CreateNewInstance();
            ModelCandidates = new ObservableCollection<IModelCandidate>();
            modelAPI.TimerUpdated += OnTimerUpdated;
            modelAPI.GetConnection().OnConnectionStateChanged += OnConnectionStateChanged;
            LoadCandidates();

            OnConnectionStateChanged();

            SelectCandidateCommand = new RelayCommand<IModelCandidate>(candidate =>
            {
                if (candidate != null)
                {
                    foreach (var c in ModelCandidates)
                        modelAPI.ChooseCandidate(c.ID);  // Deselect others

                    modelAPI.ChooseCandidate(candidate.ID);
                    SelectedCandidate = candidate;
                }
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
            // else
            // {
            //     model.WarehousePresentation.RequestUpdate(); // Update
            // }
        }

        private void OnTimerUpdated(int newTime)
        {
            TimeLeft = newTime;
        }

        private void LoadCandidates()
        {
            var candidates = modelAPI.GetModelCandidates();
            foreach (var candidate in candidates)
            {
                ModelCandidates.Add(candidate);
            }

            modelAPI.RefreshModel();
            NotifyPropertyChanged();
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}