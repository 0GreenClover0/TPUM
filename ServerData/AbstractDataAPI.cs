﻿namespace ServerData
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
                candidates.AddCandidateInformation(2, "Lawyer. Advocate for social justice reform. Prioritizes criminal justice transparency.");
                candidates.AddCandidateInformation(3, "Environmental scientist. Focus on clean energy and climate change resilience.");
                candidates.AddCandidateInformation(4, "Former mayor. Known for urban revitalization projects and housing reforms.");
                candidates.AddCandidateInformation(5, "Tech entrepreneur. Supports digital innovation and cybersecurity infrastructure.");
                candidates.AddCandidateInformation(6, "Teacher. Promotes educational reform and youth mental health initiatives.");
                candidates.AddCandidateInformation(7, "Agricultural economist. Focus on sustainable farming and rural development.");
                candidates.AddCandidateInformation(8, "Diplomat. Campaigning for international cooperation and national defense.");
                candidates.AddCandidateInformation(9, "Healthcare administrator. Committed to affordable healthcare and elder care.");
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