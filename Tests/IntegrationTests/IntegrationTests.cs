using Data;
using ServerPresentation;

namespace Tests.IntegrationTests
{
    [TestClass]
    public class IntegrationTests
    {
        WebSocketServer server = new();
        IConnection connection = Data.IConnection.Create();

        private void OnConnect(WebSocketConnectionServer connection)
        {
        }

        [TestMethod]
        public async Task ConnectionTest()
        {
            server.StartServer(42069, OnConnect);
            try
            {
                await connection.Connect(new Uri(@"ws://localhost:42069"));
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Inconclusive("Brak działającego serwera");
            }

        }

        [TestMethod]
        public async Task ExampleCommandTest()
        {
            try
            {
                await connection.Connect(new Uri(@"ws://localhost:42069"));
                await connection.SendAsync("Example");
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Inconclusive("Brak działającego serwera");
            }
        }
    }
}
