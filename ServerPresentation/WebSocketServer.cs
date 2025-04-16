using ConnectionAPI;
using ServerAPI;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace ServerPresentation
{
    public class WebSocketServer
    {
        public async Task StartServer(int p2pPort, Action<WebSocketConnection> onConnection)
        {
            Uri uri = new Uri($@"http://localhost:{p2pPort}/");
            await ServerLoop(uri, onConnection);
        }

        private async Task ServerLoop(Uri uri, Action<WebSocketConnection> onConnection)
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add(uri.ToString());
            server.Start();
            while (true)
            {
                HttpListenerContext httpContext = await server.GetContextAsync();
                if (!httpContext.Request.IsWebSocketRequest)
                {
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.Close();
                }

                HttpListenerWebSocketContext context = await httpContext.AcceptWebSocketAsync(null);
                WebSocketConnection connection = new ServerWebSocketConnection(context.WebSocket, httpContext.Request.RemoteEndPoint);
                onConnection?.Invoke(connection);
            }
        }

        internal class ServerWebSocketConnection : WebSocketConnection
        {
            private readonly IPEndPoint endPoint;
            private readonly WebSocket socket;

            public ServerWebSocketConnection(WebSocket socket, IPEndPoint endPoint)
            {
                this.socket = socket;
                this.endPoint = endPoint;
                Task.Factory.StartNew(ServerMessageLoop);
            }

            protected override Task SendTask(string message)
            {
                return socket.SendAsync(Utilities.StringToBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            public override Task DisconnectAsync()
            {
                return socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting...", CancellationToken.None);
            }

            public override string ToString()
            {
                return endPoint.ToString();
            }

            private void ServerMessageLoop()
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    ArraySegment<byte> segments = new ArraySegment<byte>(buffer);

                    WebSocketReceiveResult receiveResult;
                    try
                    {
                        receiveResult = socket.ReceiveAsync(segments, CancellationToken.None).Result;
                    }
                    catch
                    {
                        OnClose?.Invoke();
                        return;
                    }

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        OnClose?.Invoke();
                        socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure", CancellationToken.None);
                        return;
                    }

                    int count = receiveResult.Count;
                    while (!receiveResult.EndOfMessage)
                    {
                        if (count >= buffer.Length)
                        {
                            OnClose?.Invoke();
                            socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Buffer exceeded", CancellationToken.None);
                            return;
                        }

                        segments = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                        receiveResult = socket.ReceiveAsync(segments, CancellationToken.None).Result;
                        count += receiveResult.Count;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, count);
                    OnNext(message);
                }
            }
        }
    }
}