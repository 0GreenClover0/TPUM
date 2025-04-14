using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ConnectionAPI;

namespace Data
{
    public interface IConnection
    {
        public event Action? OnConnectionStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public Task Connect(Uri peerUri);
        public Task Disconnect();

        public bool IsConnected();

        public Task SendAsync(string message);

        internal class Connection : IConnection
        {
            public event Action? OnConnectionStateChanged;
            public event Action<string>? OnMessage;
            public event Action? OnError;
            public event Action? OnDisconnect;

            internal WebSocketConnection? WebSocketConnection { get; private set; }
            public async Task Connect(Uri peerUri)
            {
                WebSocketConnection = await WebSocketClient.Connect(peerUri);
                OnConnectionStateChanged?.Invoke();
                WebSocketConnection.OnMessage = (message) => OnMessage?.Invoke(message);
                WebSocketConnection.OnError = () => OnError?.Invoke();
                WebSocketConnection.OnClose = () => OnDisconnect?.Invoke();
            }

            public async Task Disconnect()
            {
                if (WebSocketConnection != null)
                {
                    await WebSocketConnection.DisconnectAsync();
                }
            }

            public bool IsConnected()
            {
                return WebSocketConnection != null;
            }

            public async Task SendAsync(string message)
            {
                if (WebSocketConnection != null)
                {
                    await WebSocketConnection.SendAsync(message);
                }
            }
        }
    }
}