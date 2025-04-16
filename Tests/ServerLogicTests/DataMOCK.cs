using ServerData;

namespace Tests.ServerLogicTests
{
    internal class TestCandidate : ICandidate
    {
        public override int ID { get; }
        public override string FullName { get; }
        public override string Party { get; }
        public override bool IsChosen { get; set; }
        public override event EventHandler<Event>? OnPropertyChanged;

        public TestCandidate(int id, string name, string party)
        {
            ID = id;
            FullName = name;
            Party = party;
            IsChosen = false;
        }

        public override void ChooseCandidate()
        {
            IsChosen = true;
        }

        public override void DeselectCandidate()
        {
            IsChosen = false;
        }
    }

    internal class TestCandidateDatabase : ICandidateDatabase
    {
        private List<ICandidate> candidates = new List<ICandidate>();

        public override void AddCandidate(ICandidate candidate)
        {
            candidates.Add(candidate);
        }

        public override void AddCandidateInformation(int candidateID, string information)
        {
            throw new NotImplementedException();
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

        public override string GetCandidateInformation(int candidateID)
        {
            throw new NotImplementedException();
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

    internal class TestDataAPI : AbstractDataAPI
    {
        private readonly TestCandidateDatabase candidates = new();
        internal IDashBoard? dashboard { get; set; }
        internal static int hardCodedBoardW = 600;
        internal static int hardCodedBoardH = 600;

        public override ICandidate? GetCandidate(int id)
        {
            return candidates.GetCandidate(id);
        }

        public override List<ICandidate> GetCandidates()
        {
            return candidates.GetCandidates();
        }

        public override void AddCandidate(int id, string name, string party)
        {
            candidates.AddCandidate(new TestCandidate(id, name, party));
        }

        public override bool RemoveCandidate(int id)
        {
            return candidates.RemoveCandidate(id);
        }

        public override void CreateDashBoard()
        {
            dashboard = IDashBoard.CreateDashBoard(hardCodedBoardW, hardCodedBoardH);
        }

        public override string GetCandidateInformation(int id)
        {
            throw new NotImplementedException();
        }
    }
}
