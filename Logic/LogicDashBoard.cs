using Data;

namespace Logic
{
    internal sealed class LogicDashBoard : AbstractLogicAPI
    {
        internal AbstractDataAPI dataApi;
        private int sessionDuration = 20;
        private int timeToEndSession = 0;
        private CancellationTokenSource? timeoutTokenSource;
        public override event Action<int>? TimerUpdated;

        private void OnTimerUpdated(int newTime)
        {
            TimerUpdated?.Invoke(newTime);
        }

        public LogicDashBoard(AbstractDataAPI dataAPI)
        {
            dataApi = dataAPI;
            CreateDashBoard();
        }

        public override void CreateDashBoard()
        {
            dataApi.CreateDashBoard();
            timeToEndSession = sessionDuration;
            OnTimerUpdated(timeToEndSession);
            StartSessionCountdown();
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

        public override bool RemoveCandidate(int id)
        {
            return dataApi.RemoveCandidate(id);
        }

        // -----------------------------------------

        private void StartSessionCountdown()
        {
            timeoutTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!timeoutTokenSource.Token.IsCancellationRequested)
                {
                    if (timeToEndSession <= 0)
                    {
                        StopSessionCountdown();
                        TimeoutSession();
                        break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), timeoutTokenSource.Token);
                    timeToEndSession--;
                    OnTimerUpdated(timeToEndSession);
                }
            }, timeoutTokenSource.Token);
        }


        private void StopSessionCountdown()
        {
            timeoutTokenSource?.Cancel();
        }

        private void TimeoutSession()
        {
            Environment.Exit(0);
        }

        ~LogicDashBoard()
        {
            StopSessionCountdown();
            TimeoutSession();
        }
    }
}

