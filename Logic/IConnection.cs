using ClientAPI;
using System;
using System.Threading.Tasks;

namespace Logic
{
    public interface IConnection
    {
        public event Action? OnConnectionStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;

        public Task Connect(Uri peerUri);
        public Task Disconnect();
        public Task SendAsync(string message);

        public bool IsConnected();

        public class Connection : Logic.IConnection
        {
            public event Action? OnConnectionStateChanged;
            public event Action<string>? OnMessage;
            public event Action? OnError;

            private readonly Data.IConnection dataConnection;

            public Connection(Data.IConnection connection)
            {
                dataConnection = connection;

                connection.OnConnectionStateChanged += () => OnConnectionStateChanged?.Invoke();
                connection.OnMessage += (message) => OnMessage?.Invoke(message);
                connection.OnError += () => OnError?.Invoke();
            }

            public async Task SendAsync(string message)
            {
                dataConnection?.SendAsync(message);
            }

            public async Task Connect(Uri peerUri)
            {
                await dataConnection.Connect(peerUri);
            }

            public async Task Disconnect()
            {
                await dataConnection.Disconnect();
            }

            public bool IsConnected()
            {
                return dataConnection.IsConnected();
            }
        }
    }
}