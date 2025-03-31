using Data;
using Logic;
using System.Collections.ObjectModel;

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

                // observerManager = logicApi.Subscribe(this); // ??????????????????
            }

            public override void ChooseCandidate(int id)
            {
                logicApi.ChooseCandidate(id);
            }

            public override void AddModelCandidate(string name, string party)
            {
                logicApi.AddNewCandidate(name, party);
            }

            public override ObservableCollection<IModelCandidate> GetModelCandidates()
            {
                return modelCandidates;
            }
        }
    }
}
