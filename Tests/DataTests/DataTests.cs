using Data;

namespace Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void ApiCreatingCandidateTest()
        {
            AbstractDataAPI testApi = AbstractDataAPI.CreateNewInstance();

            testApi.AddCandidate(0, "Robert Pattinson", "Blue Party");
            Assert.AreEqual(1, testApi.GetCandidates().Count);
        }

        [TestMethod]
        public void CreatingDashBoardTest()
        {
            IDashBoard dashboard = IDashBoard.CreateDashBoard(250, 400);

            Assert.IsNotNull(dashboard);
            Assert.AreEqual(250, dashboard.BoardWidth);
            Assert.AreEqual(400, dashboard.BoardHeight);
        }

        [TestMethod]
        public void CreatingCandidateTest()
        {
            AbstractDataAPI testApi1 = AbstractDataAPI.CreateNewInstance();
            ICandidate candidate = ICandidate.CreateCandidate(0, "Robert Pattinson", "Blue Party");
            Assert.IsNotNull(candidate);
            Assert.AreEqual(0, candidate.ID);
            Assert.AreEqual("Robert Pattinson", candidate.FullName);
            Assert.AreEqual("Blue Party", candidate.Party);
        }
    }
}



