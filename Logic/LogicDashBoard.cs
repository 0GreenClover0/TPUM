using Data;

namespace Logic
{
    internal sealed class LogicDashBoard : AbstractLogicAPI
    {
        internal AbstractDataAPI dataApi;
        private Logic.IConnection.Connection connection;
        private CancellationTokenSource? timeoutTokenSource;
        public override event Action<int>? TimerUpdated;
        public override event Action<string, int>? CandidateInfoUpdated;
        public override event Action<List<ICandidate>>? CandidatesUpdated;
        public override event Action? CandidatesRefresh;

        private void OnTimerUpdated(int newTime)
        {
            TimerUpdated?.Invoke(newTime);
            CheckForSessionTimeout(newTime);
        }

        private void OnCandidateInfoUpdated(string newInfo, int ID)
        {
            CandidateInfoUpdated?.Invoke(newInfo, ID);
        }

        private void OnCandidatesUpdated(List<ICandidate> candidates)
        {
            CandidatesRefresh?.Invoke();
        }

        private void CheckForSessionTimeout(int newTime)
        {
            if (newTime <= 0)
            {
                TimeoutSession();
            }
        }

        public LogicDashBoard(AbstractDataAPI dataAPI)
        {
            dataApi = dataAPI;
            CreateDashBoard();
        }

        public override void CreateDashBoard()
        {
            dataApi.CreateDashBoard();
            this.connection = connection ?? new Logic.IConnection.Connection(dataApi.GetConnection());
            dataApi.TimerUpdated += OnTimerUpdated;
            dataApi.CandidateInfoUpdated += OnCandidateInfoUpdated;
            dataApi.CandidatesUpdated += OnCandidatesUpdated;
        }

        public override Task SendChooseCandidate()
        {
            return dataApi.SendChooseCandidate();
        }

        public override Logic.IConnection GetConnection()
        {
            return connection;
        }

        public override List<ICandidate> GetCandidates()
        {
            return dataApi.GetCandidates();
        }

        public override void AddNewCandidate(string name, string party)
        {
            int newID = GetCandidates().Count;
            dataApi.AddCandidate(newID, name, party);
        }

        public override bool ChooseCandidate(int id)
        {
            ICandidate? candidate = dataApi.GetCandidate(id);

            if (candidate == null)
            {
                return false;
            }
            else
            {
                candidate.ChooseCandidate();
                return true;
            }
        }

        public override void MoreInfoCandidate(int id)
        {
            ICandidate? candidate = dataApi.GetCandidate(id);

            if (candidate == null)
                return;

            dataApi.MoreInfoCandidate(id);
        }

        public override bool DeselectCandidate(int id)
        {
            ICandidate? candidate = dataApi.GetCandidate(id);

            if (candidate == null)
            {
                return false;
            }
            else
            {
                candidate.DeselectCandidate();
                return true;
            }
        }

        public override bool RemoveCandidate(int id)
        {
            return dataApi.RemoveCandidate(id);
        }

        private void TimeoutSession()
        {
            Environment.Exit(0);
        }

        ~LogicDashBoard()
        {
            TimeoutSession();
        }
    }
}

