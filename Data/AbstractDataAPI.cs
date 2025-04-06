using ClientAPI;

namespace Data
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
        public abstract void CreateDashBoard();
        public abstract event Action<int>? TimerUpdated;
        public abstract Data.IConnection.Connection GetConnection();

        private class DataAPI : AbstractDataAPI
        {
            private readonly CandidateDatabase candidates;
            private Data.IConnection.Connection connection;
            internal IDashBoard? dashboard { get; set; }
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;
            public override event Action<int>? TimerUpdated;

            public DataAPI()
            {
                this.connection = connection ?? new Data.IConnection.Connection();
                connection.OnMessage += OnMessage;
                candidates = new CandidateDatabase();
            }

            public override Data.IConnection.Connection GetConnection()
            {
                return connection;
            }

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
                candidates.AddCandidate(new Candidate(id, name, party));
            }

            public override bool RemoveCandidate(int id)
            {
                return candidates.RemoveCandidate(id);
            }

            public override void CreateDashBoard()
            {
                dashboard = IDashBoard.CreateDashBoard(hardCodedBoardW, hardCodedBoardH);
            }

            private async void OnMessage(string message)
            {
                if (connection == null)
                    return;

                Console.WriteLine($"New message: {message}");

                JsonSerializer serializer = new JsonSerializer();
                string header = serializer.GetResponseHeader(message);

                //if (header == UpdateCandidatesResponse.StaticHeader)
                {
                    //UpdateCandidatesResponse response = serializer.Deserialize<UpdateCandidatesResponse>(message);

                    // Loop candidates or update ObservableCollection here
                    //foreach (var c in response.Candidates)
                    {
                        //Console.WriteLine($"Candidate: {c.FullName} - Chosen: {c.IsChosen}");
                    }
                }
                if (header == TimerResponse.StaticHeader)
                {
                    TimerResponse timer = serializer.Deserialize<TimerResponse>(message);
                    TimerUpdated.Invoke(timer.NewTime);
                }
            }
        }
    }
}