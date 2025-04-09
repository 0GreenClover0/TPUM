using ClientAPI;

namespace Data
{
    public abstract class AbstractDataAPI
    {
        public static AbstractDataAPI CreateNewInstance()
        {
            return new DataAPI();
        }

        public static AbstractDataAPI CreateNewInstance(IConnection connection)
        {
            return new DataAPI(connection);
        }

        public abstract int GetSessionTime();
        public abstract ICandidate? GetCandidate(int id);
        public abstract List<ICandidate> GetCandidates();
        public abstract void AddCandidate(int id, string name, string party);
        public abstract bool RemoveCandidate(int id);
        public abstract void CreateDashBoard();
        public abstract event Action<int>? TimerUpdated;
        public abstract IConnection GetConnection();
        public abstract Task SendChooseCandidate();

        private class DataAPI : AbstractDataAPI
        {
            private readonly CandidateDatabase candidates;
            private IConnection connection;
            internal IDashBoard? dashboard { get; set; }
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;
            internal int sessionTime = 0;
            public override event Action<int>? TimerUpdated;

            public DataAPI()
            {
                this.connection = connection ?? new Data.IConnection.Connection();
                connection.OnMessage += OnMessage;
                candidates = new CandidateDatabase();
            }

            public DataAPI(IConnection connection)
            {
                this.connection = connection;
                connection.OnMessage += OnMessage;
                candidates = new CandidateDatabase();
            }

            public override int GetSessionTime()
            {
                return sessionTime;
            }

            public override IConnection GetConnection()
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

            public override async Task SendChooseCandidate()
            {
                if (connection == null)
                    return;

                Console.WriteLine($"Sending candidate info...");

                ChooseCandidateCommand serverCommand = new ChooseCandidateCommand();
                var candidates = GetCandidates();
                List<CandidateDTO> cDTOs = new List<CandidateDTO>();

                foreach (var c in candidates)
                {
                    CandidateDTO candidateDTO = new CandidateDTO(c.ID, c.FullName, c.Party, c.IsChosen);
                    cDTOs.Add(candidateDTO);
                }

                serverCommand.Candidates = cDTOs.ToArray();
                JsonSerializer serializer = new JsonSerializer();
                string commandJson = serializer.Serialize(serverCommand);
                Console.WriteLine(commandJson);

                await connection.SendAsync(commandJson);
            }

            private async void OnMessage(string message)
            {
                if (connection == null)
                    return;

                Console.WriteLine($"New message: {message}");

                if (message == ServerCommand.ClosedConnectionHeader)
                {
                    TimerUpdated.Invoke(0);
                    sessionTime = 0;
                    return;
                }

                JsonSerializer serializer = new JsonSerializer();
                string header = serializer.GetResponseHeader(message);

                if (header == TimerResponse.StaticHeader)
                {
                    TimerResponse timer = serializer.Deserialize<TimerResponse>(message);
                    TimerUpdated?.Invoke(timer.NewTime);
                    sessionTime = timer.NewTime;
                }
            }
        }
    }
}