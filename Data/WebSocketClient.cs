using ConnectionAPI;
using System.Net.WebSockets;
using System.Text;

namespace Data
{
    internal class WebSocketClient
    {
        public static ArraySegment<byte> StringToBytes(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(buffer);
        }

        public static async Task<WebSocketConnection> Connect(Uri peer)
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(peer, CancellationToken.None);
            switch (clientWebSocket.State)
            {
                case WebSocketState.Open:
                    WebSocketConnection socket = new ClientWebSocketConnection(clientWebSocket, peer);
                    return socket;
                default:
                    throw new WebSocketException($"Websocket connection error: {clientWebSocket.State}");
            }
        }

        internal class ClientWebSocketConnection : WebSocketConnection
        {
            private readonly ClientWebSocket clientWebSocket;
            private readonly Uri peer;

            public ClientWebSocketConnection(ClientWebSocket clientWebSocket, Uri peer)
            {
                this.clientWebSocket = clientWebSocket;
                this.peer = peer;
                Task.Factory.StartNew(ClientMessageLoop);
            }

            protected override Task SendTask(string message)
            {
                return clientWebSocket.SendAsync(StringToBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            public override Task DisconnectAsync()
            {
                return clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting...", CancellationToken.None);
            }

            public override string ToString()
            {
                return peer.ToString();
            }

            private void ClientMessageLoop()
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(buffer);

                    WebSocketReceiveResult result;
                    try
                    {
                        result = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                    }
                    catch
                    {
                        OnClose?.Invoke();
                        OnMessage?.Invoke(ConnectionAPI.ServerStatics.ClosedConnection);
                        return;
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        OnClose?.Invoke();
                        clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure", CancellationToken.None).Wait();
                        return;
                    }

                    int count = result.Count;
                    while (!result.EndOfMessage)
                    {
                        if (count >= buffer.Length)
                        {
                            OnClose?.Invoke();
                            clientWebSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Buffer exceeded",
                                    CancellationToken.None)
                                .Wait();
                            return;
                        }

                        segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                        result = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                        count += result.Count;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, count);
                    OnMessage?.Invoke(message);
                }
            }
        }
    }
}
