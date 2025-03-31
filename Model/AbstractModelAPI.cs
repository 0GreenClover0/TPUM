using Logic;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

        internal sealed class ModelAPI : AbstractModelAPI
        {
            private readonly AbstractLogicAPI logicApi;
            private readonly ObservableCollection<IModelCandidate> modelCandidates = new ObservableCollection<IModelCandidate>();
            private readonly IDisposable? observerManager;

            internal ModelAPI(AbstractLogicAPI logicAPI)
            {
                this.logicApi = logicAPI;

                // 1
                logicApi.AddNewCandidate("Donatan Trumpet", "Red Party");
                logicApi.AddNewCandidate("Kamaleona Harrison", "Blue Party");

                // 2                
                RefreshModel();

                // TODO: Solve this
                // if 1 & 2 are called together, it causes stack overflow
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

            public override ObservableCollection<IModelCandidate> GetModelCandidates()
            {
                return modelCandidates;
            }

            private void RefreshModel()
            {
                modelCandidates.Clear();
                var candidates = logicApi.GetCandidates();

                foreach (var c in candidates)
                {
                    modelCandidates.Add(new ModelCandidate(c.ID, c.FullName, c.Party));
                }
            }
        }
    }
}
