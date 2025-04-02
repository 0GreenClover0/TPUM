using Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Model
{
    public abstract class AbstractModelAPI
    {
        public static AbstractModelAPI CreateNewInstance(AbstractLogicAPI? LogicAPI = default)
        {
            return new ModelAPI(LogicAPI == null ? AbstractLogicAPI.CreateNewInstance() : LogicAPI);
        }

        public abstract ObservableCollection<IModelCandidate> GetModelCandidates();
        public abstract void AddModelCandidate(string name, string party);
        public abstract void ChooseCandidate(int id);
        public abstract void RefreshModel();
        public abstract event Action<int>? TimerUpdated;

        internal sealed class ModelAPI : AbstractModelAPI
        {
            public ObservableCollection<IModelCandidate> ModelCandidates { get; set; }
            public event PropertyChangedEventHandler? PropertyChanged;
            public override event Action<int>? TimerUpdated;

            public override ObservableCollection<IModelCandidate> GetModelCandidates()
            {
                return ModelCandidates;
            }

            private readonly AbstractLogicAPI logicApi;
            private readonly IDisposable? observerManager;

            internal ModelAPI(AbstractLogicAPI logicAPI)
            {
                this.logicApi = logicAPI;
                ModelCandidates = new ObservableCollection<IModelCandidate>();

                logicApi.TimerUpdated += OnTimerUpdated;

                logicApi.AddNewCandidate("Donatan Trumpet", "Red Party");
                logicApi.AddNewCandidate("Kamaleona Harrison", "Blue Party");

                RefreshModel();
            }

            private void OnTimerUpdated(int newTime)
            {
                TimerUpdated?.Invoke(newTime);
            }

            public override void ChooseCandidate(int id)
            {
                logicApi.ChooseCandidate(id);
                RefreshModel();
            }

            public override void AddModelCandidate(string name, string party)
            {
                logicApi.AddNewCandidate(name, party);
                RefreshModel();
            }

            public override void RefreshModel()
            {
                ModelCandidates?.Clear();
                var candidates = logicApi.GetCandidates();

                foreach (var c in candidates)
                {
                    ModelCandidates.Add(new ModelCandidate(c.ID, c.FullName, c.Party));
                }
                NotifyPropertyChanged();
            }

            private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
