namespace Tests.ServerLogicTests
{
    internal class ConnectionMOCK : Data.IConnection
    {
        public event Action? OnConnectionStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public async Task Connect(Uri peerUri)
        {
            throw new NotImplementedException();
        }

        public async Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(string message)
        {
            OnMessage.Invoke(message);

            await Task.Delay(0);
        }
    }
}
