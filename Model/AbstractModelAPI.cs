using Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public abstract void MoreInfoCandidate(int id);
        public abstract void DeselectCandidate(int id);
        public abstract void RefreshModel();
        public abstract Model.IConnection GetConnection();
        public abstract Task SendChooseCandidate();
        public abstract event Action<int>? TimerUpdated;
        public abstract event Action<string, int>? CandidateInfoUpdated;

        internal sealed class ModelAPI : AbstractModelAPI
        {
            public ObservableCollection<IModelCandidate> ModelCandidates { get; set; }
            public event PropertyChangedEventHandler? PropertyChanged;
            public override event Action<int>? TimerUpdated;
            public override event Action<string, int>? CandidateInfoUpdated;
            private Model.IConnection.Connection connection;

            public override ObservableCollection<IModelCandidate> GetModelCandidates()
            {
                return ModelCandidates;
            }

            private readonly AbstractLogicAPI logicApi;
            private readonly IDisposable? observerManager;

            internal ModelAPI(AbstractLogicAPI logicAPI)
            {
                this.logicApi = logicAPI;
                this.connection = connection ?? new Model.IConnection.Connection(logicAPI.GetConnection());
                ModelCandidates = new ObservableCollection<IModelCandidate>();

                logicApi.TimerUpdated += OnTimerUpdated;
                logicAPI.CandidateInfoUpdated += OnCandidateInfoUpdated;

                logicApi.AddNewCandidate("Donatan Trumpet", "Red Party");
                logicApi.AddNewCandidate("Kamaleona Harrison", "Blue Party");

                RefreshModel();
            }

            public override Model.IConnection GetConnection()
            {
                return connection;
            }

            public override Task SendChooseCandidate()
            {
                return logicApi.SendChooseCandidate();
            }

            private void OnTimerUpdated(int newTime)
            {
                TimerUpdated?.Invoke(newTime);
            }

            private void OnCandidateInfoUpdated(string newInfo, int ID)
            {
                CandidateInfoUpdated?.Invoke(newInfo, ID);
            }

            public override void ChooseCandidate(int id)
            {
                logicApi.ChooseCandidate(id);
                RefreshModel();
            }

            public override void MoreInfoCandidate(int id)
            {
                logicApi.MoreInfoCandidate(id);
            }

            public override void DeselectCandidate(int id)
            {
                logicApi.DeselectCandidate(id);
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
