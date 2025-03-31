namespace Data
{
    public abstract class ICandidateDatabase
    {
        public abstract List<ICandidate> GetCandidates();
        public abstract ICandidate? GetCandidate(int id);
        public abstract void AddCandidate(ICandidate candidate);
        public abstract bool RemoveCandidate(int id);
    }
}
