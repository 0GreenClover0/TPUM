using System;
using System.Threading.Tasks;
using Logic;

namespace Model
{
    public interface IConnection
    {
        public event Action? OnConnectionStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;

        public Task Connect(Uri peerUri);
        public Task Disconnect();

        public bool IsConnected();

        internal class Connection : Model.IConnection
        {
            public event Action? OnConnectionStateChanged;
            public event Action<string>? OnMessage;
            public event Action? OnError;

            private readonly Logic.IConnection connectionService;

            public Connection(Logic.IConnection connectionService)
            {
                this.connectionService = connectionService;
                this.connectionService.OnConnectionStateChanged += () => OnConnectionStateChanged?.Invoke();
                this.connectionService.OnMessage += (message) => OnMessage?.Invoke(message);
                this.connectionService.OnError += () => OnError?.Invoke();
            }

            public async Task SendAsync(string message)
            {
                connectionService?.SendAsync(message);
            }

            public async Task Connect(Uri peerUri)
            {
                await connectionService.Connect(peerUri);
            }

            public async Task Disconnect()
            {
                await connectionService.Disconnect();
            }

            public bool IsConnected()
            {
                return connectionService.IsConnected();
            }
        }
    }
}