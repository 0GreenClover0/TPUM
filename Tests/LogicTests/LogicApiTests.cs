using Logic;

namespace Tests
{ 
    [TestClass]
    public class LogicApiTests
    {
        [TestMethod]
        public void CreateNewInstanceTest()
        {
            LogicApi logicApi1 = LogicApi.ApiInstance;

            Assert.IsNotNull(logicApi1);
        }
        [TestMethod]
        public void GenerateBallsTest()
        {
            LogicApi logicApi = LogicApi.ApiInstance;
            int numberOfBalls = 20;

            logicApi.GenerateBalls(numberOfBalls);
            List<IBall> ballList1 = logicApi.GetBallList();
            Assert.AreEqual(ballList1.Count, numberOfBalls);
        }
    }
}
