﻿using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelMainWindow : INotifyPropertyChanged
    {
        private readonly AbstractModelAPI modelAPI;
        private IModelCandidate selectedCandidate;
        private int timeLeft = 20;

        public ObservableCollection<CandidateAndInfo> CandidatesAndInfo { get; set; }
        public ObservableCollection<IModelCandidate> ModelCandidates { get; set; }
        public ICommand SelectCandidateCommand { get; }
        public ICommand MoreInfoCommand { get; }
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
            ModelCandidates = [];
            CandidatesAndInfo = [];
            modelAPI.TimerUpdated += OnTimerUpdated;
            modelAPI.CandidateInfoUpdated += OnCandidateInfoUpdated;
            modelAPI.GetConnection().OnConnectionStateChanged += OnConnectionStateChanged;
            LoadCandidates();
            LoadCandidatesAndInfo();

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

        private void LoadCandidatesAndInfo()
        {
            foreach (var candidate in ModelCandidates)
            {
                CandidatesAndInfo.Add(new CandidateAndInfo(candidate));
            }
        }

        private void LoadCandidateInfo(string newInfo, int ID)
        {
            CandidatesAndInfo[ID].Info = newInfo;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}