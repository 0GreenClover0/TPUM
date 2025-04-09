﻿using ServerLogic;

namespace Tests.ServerLogicTests
{
    internal class ServerLogicTests
    {
        [TestMethod]
        public void LogicAPICreateNewInstanceTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            Assert.IsNotNull(testLogicApi);
        }

        [TestMethod]
        public void LogicAPICreateCandidateTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.AddNewCandidate("Robert Pattinson", "Blue Party");
            Assert.AreEqual(1, testLogicApi.GetCandidates().Count);
        }

        [TestMethod]
        public void LogicAPICreateDashBoardTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateDashBoard();
            Assert.IsNotNull(testDataApi.dashboard);
        }
    }
}
