namespace ServerData
{
    public abstract class AbstractDataAPI
    {
        public static AbstractDataAPI CreateNewInstance()
        {
            return new DataAPI();
        }

        public abstract ICandidate? GetCandidate(int id);
        public abstract List<ICandidate> GetCandidates();
        public abstract void AddCandidate(int id, string name, string party);
        public abstract bool RemoveCandidate(int id);
        public abstract string GetCandidateInformation(int id);
        public abstract void CreateDashBoard();

        internal class DataAPI : AbstractDataAPI
        {
            private readonly CandidateDatabase candidates;
            internal IDashBoard? dashboard { get; set; }
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;

            public DataAPI()
            {
                candidates = new CandidateDatabase();
                candidates.AddCandidateInformation(0, "Economist. Former Minister of Finance. Higher salaries for teachers and nurses.");
                candidates.AddCandidateInformation(1, "Civil engineer. Long-time local government official. Expansion of road infrastructure.");
            }

            public override ICandidate? GetCandidate(int id)
            {
                lock (candidates)
                {
                    return candidates.GetCandidate(id);
                }
            }

            public override List<ICandidate> GetCandidates()
            {
                lock (candidates)
                {
                    return candidates.GetCandidates();
                }
            }

            public override void AddCandidate(int id, string name, string party)
            {
                lock (candidates)
                {
                    candidates.AddCandidate(new Candidate(id, name, party));
                }
            }

            public override bool RemoveCandidate(int id)
            {
                lock (candidates)
                {
                    return candidates.RemoveCandidate(id);
                }
            }

            public override string GetCandidateInformation(int id)
            {
                lock (candidates)
                {
                    return candidates.GetCandidateInformation(id);
                }
            }

            public override void CreateDashBoard()
            {
                dashboard = IDashBoard.CreateDashBoard(hardCodedBoardW, hardCodedBoardH);
            }
        }
    }
}