using Data;

namespace Logic
{
    internal sealed class LogicDashBoard : AbstractLogicAPI
    {
        internal AbstractDataAPI dataApi;
        private Logic.IConnection.Connection connection;
        private CancellationTokenSource? timeoutTokenSource;
        public override event Action<int>? TimerUpdated;

        private void OnTimerUpdated(int newTime)
        {
            TimerUpdated?.Invoke(newTime);
            CheckForSessionTimeout(newTime);
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
        }

        public override Task SendChooseCandidate()
        {
            return dataApi.SendChooseCandidate();
        }

        public override Logic.IConnection.Connection GetConnection()
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

