using Data;
using System.Numerics;

namespace Logic
{
    public abstract class AbstractLogicAPI // : IObserver<ICandidate>, IObservable<int>
    {
        public abstract List<ICandidate> GetCandidates();
        public abstract bool ChooseCandidate(int id);
        public abstract void MoreInfoCandidate(int id);
        public abstract bool DeselectCandidate(int id);
        public abstract void AddNewCandidate(string name, string party);
        public abstract bool RemoveCandidate(int id);
        public abstract void CreateDashBoard();
        public abstract Logic.IConnection GetConnection();
        public abstract Task SendChooseCandidate();
        public abstract event Action<int>? TimerUpdated;
        public abstract event Action<string, int>? CandidateInfoUpdated;
        public abstract event Action<List<ICandidate>>? CandidatesUpdated;
        public abstract event Action? CandidatesRefresh;

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
