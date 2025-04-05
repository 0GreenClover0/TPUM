using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientAPI;
using ServerData;
using ServerLogic;

namespace ServerPresentation
{
    internal class Program
    {
        private readonly AbstractLogicAPI logicAPI;

        private WebSocketConnection? webSocketConnection;

        private Program(AbstractLogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
            logicAPI.TimerUpdated += OnTimerUpdated; // Session timeout
        }


        private async Task StartConnection()
        {
            while (true)
            {
                Console.WriteLine("Connecting...");
                await WebSocketServer.StartServer(42069, OnConnect);
            }
        }

        private void OnConnect(WebSocketConnection connection)
        {
            Console.WriteLine($"Connected to {connection}");

            connection.OnMessage = OnMessage;
            connection.OnError = OnError;
            connection.OnClose = OnClose;

            webSocketConnection = connection;
        }

        private async void OnMessage(string message)
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"New message: {message}");

            JsonSerializer serializer = new JsonSerializer();
            if (serializer.GetCommandHeader(message) == ChooseCandidateCommand.StaticHeader)
            {
                ChooseCandidateCommand chooseCandidateCommand = serializer.Deserialize<ChooseCandidateCommand>(message);
                Task task = Task.Run(ChooseCandidate);
            }
        }

        private async Task ChooseCandidate()
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"Sending candidate info...");

            UpdateCandidatesResponse serverResponse = new UpdateCandidatesResponse();
            List<ICandidate> candidates = logicAPI.GetCandidates();
            List<CandidateDTO> cDTOs = new List<CandidateDTO>();

            foreach (var c in candidates)
            {
                CandidateDTO candidateDTO = new CandidateDTO(c.ID, c.FullName, c.Party, c.IsChosen);
                cDTOs.Add(candidateDTO);
            }

            serverResponse.Candidates = cDTOs.ToArray();
            JsonSerializer serializer = new JsonSerializer();
            string responseJson = serializer.Serialize(serverResponse);
            Console.WriteLine(responseJson);

            await webSocketConnection.SendAsync(responseJson);
        }

        private async void OnTimerUpdated(int timeLeft)
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"Time left: {timeLeft}");
            TimerResponse timerResponse = new TimerResponse();
            timerResponse.NewTime = timeLeft;

            JsonSerializer serializer = new JsonSerializer();
            string responseJson = serializer.Serialize(timerResponse);
            Console.WriteLine(responseJson);

            await webSocketConnection.SendAsync(responseJson);
        }

        private void OnError()
        {
            Console.WriteLine($"Connection error");
        }

        private async void OnClose()
        {
            Console.WriteLine($"Connection closed");
            webSocketConnection = null;
        }


        private static async Task Main(string[] args)
        {
            Program program = new Program(AbstractLogicAPI.CreateNewInstance());
            await program.StartConnection();
        }
    }
}