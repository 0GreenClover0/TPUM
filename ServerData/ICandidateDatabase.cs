namespace ServerData
{
    public abstract class ICandidateDatabase
    {
        public abstract List<ICandidate> GetCandidates();
        public abstract ICandidate? GetCandidate(int id);
        public abstract void AddCandidateInformation(int candidateID, string information);
        public abstract string GetCandidateInformation(int candidateID);
        public abstract void AddCandidate(ICandidate candidate);
        public abstract bool RemoveCandidate(int id);
    }
}
