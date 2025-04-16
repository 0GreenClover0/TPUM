using ConnectionAPI;

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
        public abstract event Action<string, int>? CandidateInfoUpdated;
        public abstract IConnection GetConnection();
        public abstract Task SendChooseCandidate();
        public abstract Task MoreInfoCandidate(int id);

        internal class DataAPI : AbstractDataAPI
        {
            private readonly CandidateDatabase candidates;
            private IConnection connection;
            internal IDashBoard? dashboard { get; set; }
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;
            internal int sessionTime = 0;
            public override event Action<int>? TimerUpdated;
            public override event Action<string, int>? CandidateInfoUpdated;

            private readonly object _sessionTimeLock = new();
            private readonly object _candidateInfoLock = new();

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
                lock (_sessionTimeLock)
                {
                    return sessionTime;
                }
            }

            public override IConnection GetConnection()
            {
                return connection;
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

            public override void CreateDashBoard()
            {
                dashboard = IDashBoard.CreateDashBoard(hardCodedBoardW, hardCodedBoardH);
            }

            public override async Task SendChooseCandidate()
            {
                if (connection == null)
                    return;

                Console.WriteLine($"Sending candidate info...");

                var candidates = GetCandidates();
                List<CandidateDTO> cDTOs = new List<CandidateDTO>();

                foreach (var c in candidates)
                {
                    CandidateDTO candidateDTO = new CandidateDTO { ID = c.ID, FullName = c.FullName, Party = c.Party, IsChosen = c.IsChosen };
                    cDTOs.Add(candidateDTO);
                }

                ChooseCandidateCommand serverCommand = new ChooseCandidateCommand
                {
                    Header = ServerStatics.UpdateCandidates,
                    Candidates = cDTOs
                };

                JsonSerializer serializer = new JsonSerializer();
                string commandJson = serializer.Serialize(serverCommand);
                Console.WriteLine(commandJson);

                await connection.SendAsync(commandJson);
            }

            public override async Task MoreInfoCandidate(int id)
            {
                if (connection == null)
                    return;

                Console.WriteLine($"Getting candidate info...");

                var candidate = GetCandidate(id);

                CandidateDTO cDTO = new CandidateDTO
                {
                    ID = candidate.ID,
                    FullName = candidate.FullName,
                    Party = candidate.Party,
                    IsChosen = candidate.IsChosen
                };

                MoreInfoCandidateCommand serverCommand = new MoreInfoCandidateCommand
                {
                    Header = ServerStatics.MoreInfoCandidate,
                    Candidate = cDTO
                };

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

                if (message == ServerStatics.ClosedConnection)
                {
                    lock (_sessionTimeLock)
                    {
                        TimerUpdated.Invoke(0);
                        sessionTime = 0;
                    }

                    return;
                }

                JsonSerializer serializer = new JsonSerializer();
                string header = serializer.GetResponseHeader(message);

                if (header == ServerStatics.TimerChanged)
                {
                    lock (_sessionTimeLock)
                    {
                        TimerResponse timer = serializer.Deserialize<TimerResponse>(message);
                        TimerUpdated?.Invoke(timer.NewTime);
                        sessionTime = timer.NewTime;
                    }
                }
                else if (header == ServerStatics.CandidateInfo)
                {
                    lock (_candidateInfoLock)
                    {
                        CandidateInfoResponse candidateInfoResponse = serializer.Deserialize<CandidateInfoResponse>(message);
                        CandidateInfoUpdated?.Invoke(candidateInfoResponse.Information, candidateInfoResponse.ID);
                    }
                }
            }
        }
    }
}
