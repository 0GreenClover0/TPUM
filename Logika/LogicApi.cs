namespace Logic
{
    public abstract class LogicApi
    {
        private static LogicApi _apiInstance = new Logic();

        public static LogicApi ApiInstance
        {
            get { return _apiInstance; }
        }

        public abstract void GenerateBalls(int number);

        public abstract void CreateThreads();

        public abstract void StopThreads();

        public abstract List<IBall> GetBallList();

        internal sealed class Logic : LogicApi
        {
            private bool stopThreads = false;

            private List<IBall> ballList = new List<IBall>();

            public override List<IBall> GetBallList()
            {
                return ballList;
            }
            public override void GenerateBalls(int number)
            {
                List<IBall> ballList = GetBallList();
                ballList.Clear();
                Random random = new Random();

                for (int i = 0; i < number; i++)
                {
                    int x = random.Next(10, 590);
                    int y = random.Next(10, 590);
                    IBall ball = IBall.CreateBall(x, y);
                    ballList.Add(ball);
                }
            }

            public override void CreateThreads()
            {
                List<IBall> ballList = GetBallList();
                stopThreads = false;

                foreach (IBall ball in ballList)
                {
                    Thread thread = new Thread(() =>
                    {
                        Random random = new Random();
                        int counter = 0;
                        int dx = random.Next(-5, 5);
                        int dy = random.Next(-5, 5);
                        while (!stopThreads)
                        {
                            counter += random.Next(1, 5);
                            if (counter >= 50)
                            {
                                dx = random.Next(-5, 6);
                                dy = random.Next(-5, 6);
                                counter = 0;
                            }
                            if (ball.x < 40)
                            {
                                dx = random.Next(5, 10);
                                dy = random.Next(-5, 5);
                            }
                            if (ball.x > 560)
                            {
                                dx = random.Next(-10, -5);
                                dy = random.Next(-5, 5);
                            }
                            if (ball.y < 40)
                            {
                                dx = random.Next(-5, 5);
                                dy = random.Next(5, 10);
                            }
                            if (ball.y > 560)
                            {
                                dx = random.Next(-5, 5);
                                dy = random.Next(-10, -5);
                            }

                            ball.x += dx;
                            ball.y += dy;

                            Thread.Sleep(16);
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }

            public override void StopThreads()
            {
                stopThreads = true;
            }
        }
    }
}
