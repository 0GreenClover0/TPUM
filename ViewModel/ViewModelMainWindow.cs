using Model;
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

        public ViewModelMainWindow()
        {
            modelAPI = AbstractModelAPI.CreateNewInstance();
            //ModelCandidates = new ObservableCollection<IModelCandidate>();
            //LoadCandidates();

            SelectCandidateCommand = new RelayCommand<IModelCandidate>(candidate =>
            {
                if (candidate != null)
                {
                    foreach (var c in ModelCandidates)
                        c.IsChosen = false;  // Deselect others

                    candidate.IsChosen = true;
                    SelectedCandidate = candidate;
                }
            });
        }

        private void LoadCandidates()
        {
            var candidates = modelAPI.GetModelCandidates();
            foreach (var candidate in candidates)
            {
                ModelCandidates.Add(candidate);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}