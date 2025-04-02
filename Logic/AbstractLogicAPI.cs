using Data;
using System.Numerics;

namespace Logic
{
    public abstract class AbstractLogicAPI // : IObserver<ICandidate>, IObservable<int>
    {
        public abstract List<ICandidate> GetCandidates();
        public abstract bool ChooseCandidate(int id);
        public abstract void AddNewCandidate(string name, string party);
        public abstract bool RemoveCandidate(int id);
        public abstract void CreateDashBoard();
        public abstract event Action<int>? TimerUpdated;

        // public abstract void OnCompleted();
        // public abstract void OnError(Exception error);
        // public abstract void OnNext(ICandidate value);
        // public abstract IDisposable Subscribe(IObserver<int> observerObj);

        public static AbstractLogicAPI CreateNewInstance(AbstractDataAPI? DataAPI = default)
        {
            return new LogicDashBoard(DataAPI == null ? AbstractDataAPI.CreateNewInstance() : DataAPI);
        }
    }
}
