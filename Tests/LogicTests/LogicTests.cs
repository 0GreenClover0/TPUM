using Data;
using Logic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace Tests
{
    [TestClass]
    public class LogicTests
    {
        internal class TestDataAPI : AbstractDataAPI
        {
            internal float preDeterminedMass = 12;
            internal float preDeterminedRadius = 12;
            internal static int hardCodedBoardW = 1000;
            internal static int hardCodedBoardH = 1000;
            internal Random RNG = new Random();
            internal ILogger? logger = null;
            internal bool CreateBoardFunctionUsed { get; set; }
            internal int boardW { get; set; }
            internal int boardH { get; set; }

            public override TestBall CreateBall(int id)
            {
                double x = RNG.NextDouble() * (GetBoardW() - 2 * preDeterminedRadius) + preDeterminedRadius;
                double y = RNG.NextDouble() * (GetBoardH() - 2 * preDeterminedRadius) + preDeterminedRadius;
                Vector2 test = new Vector2((float)x, (float)y);
                x = RNG.NextDouble() * 10 - 4;
                y = RNG.NextDouble() * 10 - 4;
                Vector2 testV = new Vector2((float)x, (float)y);
                return new TestBall(id, preDeterminedMass, preDeterminedRadius, test, testV, 10, logger);
            }

            public override void CreateLogger()
            {
                this.logger = ILogger.CreateLogger();
            }

            public override void CreateBoard()
            {
                this.boardW = hardCodedBoardW;
                this.boardH = hardCodedBoardH;
                this.CreateBoardFunctionUsed = true;
            }

            public override int GetBoardH()
            {
                return this.boardH;
            }

            public override int GetBoardW()
            {
                return this.boardW;
            }
        }

        internal class TestBall : ICandidate
        {
            private Vector2 PrivCoords;
            public override int BallID { get; }
            public override Vector2 Coordinates { get; set; }
            public override Vector2 VelVector { get; set; }
            public override float DeltaTime { get; set; }
            public override float Mass { get; set; }
            public override bool StopTask { get; set; }
            public override float Radius { get; set; }
            public override bool StartMoving { get; set; }
            public override bool IsInACollision { get; set; }
            private Stopwatch stopwatch;
            private ILogger logger;
            internal IObserver<ICandidate>? ObserverObject;

            public override IDisposable Subscribe(IObserver<ICandidate> observerObj)
            {
                this.ObserverObject = observerObj;
                return new ObserverManager(observerObj);
            }
            private class ObserverManager : IDisposable
            {
                IObserver<ICandidate>? obs;

                public ObserverManager(IObserver<ICandidate> observerObj)
                {
                    this.obs = observerObj;
                }

                public void Dispose()
                {
                    this.obs = null;
                }
            }
            public override void Dispose()
            {
                this.StopTask = true;
            }

            public TestBall(int ballID, float mass, float radius, Vector2 coords, Vector2 vector, float delta, ILogger? logger)
            {
                this.BallID = ballID;
                this.Mass = mass;
                this.PrivCoords = coords;
                this.VelVector = vector;
                this.DeltaTime = delta;
                this.StopTask = false;
                this.StartMoving = false;
                this.IsInACollision = false;
                this.Radius = radius;
                stopwatch = new Stopwatch();
                Task.Run(Moving);
            }
            private async void Moving()
            {
                int delay = 0;
                while (!this.StopTask)
                {
                    stopwatch.Restart();
                    stopwatch.Start();

                    if (this.StartMoving)
                    {
                        this.UpdateCoords();
                        if (this.ObserverObject != null)
                        {
                            this.ObserverObject.OnNext(this);
                        }
                        this.IsInACollision = false;
                    }
                    if (this.logger != null)
                    {
                        logger.AddBallToQueue(this);
                    }

                    stopwatch.Stop();
                    if (this.DeltaTime - stopwatch.ElapsedMilliseconds < 0)
                    {
                        delay = 10;
                    }
                    else
                    {
                        delay = (int)this.DeltaTime - (int)stopwatch.ElapsedMilliseconds;
                    }
                    await Task.Delay(delay);
                }
            }
            private void UpdateCoords()
            {
                this.PrivCoords += this.VelVector * this.DeltaTime;
            }
        }

        [TestMethod]
        public void LogicAPICreateNewInstanceTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            Assert.IsNotNull(testLogicApi);
        }
        [TestMethod]
        public void LogicAPICreateBallsTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateBoard();
            testLogicApi.CreateBalls(5);
            List<List<double>> ballCoords = testLogicApi.GetBallsCoordsAndRadius();
            Assert.AreEqual(ballCoords.Count, 5);
        }

        [TestMethod]
        public void LogicAPICreateBoardTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateBoard();
            Assert.AreEqual(true, testDataApi.CreateBoardFunctionUsed);
            Assert.AreEqual(1000, testDataApi.GetBoardH());
            Assert.AreEqual(1000, testDataApi.GetBoardW());
        }
        [TestMethod]
        public void LogicAPIClearBoardTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateBoard();
            testLogicApi.CreateBalls(5);
            List<List<double>> ballCoords = testLogicApi.GetBallsCoordsAndRadius();
            Assert.AreEqual(ballCoords.Count, 5);
            testLogicApi.ClearBoard();
            ballCoords = testLogicApi.GetBallsCoordsAndRadius();
            Assert.AreEqual(ballCoords.Count, 0);
        }
    }
}



