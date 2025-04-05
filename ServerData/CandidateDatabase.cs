namespace ServerData
{
    internal class CandidateDatabase : ICandidateDatabase
    {
        private List<ICandidate> candidates = new List<ICandidate>();

        public override void AddCandidate(ICandidate candidate)
        {
            candidates.Add(candidate);
        }

        public override ICandidate? GetCandidate(int id)
        {
            if (candidates.ElementAt(id) == null)
            {
                return null;
            }
            else
            {
                return candidates.ElementAt(id);
            }
        }

        public override List<ICandidate> GetCandidates()
        {
            return candidates;
        }

        public override bool RemoveCandidate(int id)
        {
            if (candidates.ElementAt(id) == null)
            {
                return false;
            }
            else
            {
                candidates.RemoveAt(id);
                return true;
            }
        }
    }
}
