﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using ServerAPI;
using ServerData;
using ServerLogic;

namespace ServerPresentation
{
    internal class Program : IObserver<string>
    {
        private readonly AbstractLogicAPI logicAPI;

        private WebSocketConnectionServer? webSocketConnection;

        private Program(AbstractLogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
            logicAPI.TimerUpdated += OnTimerUpdated; // Session timeout
            logicAPI.CandidatesEmit += OnCandidatesEmit; // Candidates emission
        }

        private async Task StartConnection()
        {
            WebSocketServer server = new();

            while (true)
            {
                Console.WriteLine("Connecting...");
                await server.StartServer(42069, OnConnect);
            }
        }

        private async void OnCandidatesEmit()
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"Candidates refreshed and sent to client.");

            var candidates = logicAPI.GetCandidates();
            List<CandidateDTO> cDTOs = new List<CandidateDTO>();

            foreach (var c in candidates)
            {
                CandidateDTO candidateDTO = new CandidateDTO { ID = c.ID, FullName = c.FullName, Party = c.Party, IsChosen = c.IsChosen };
                cDTOs.Add(candidateDTO);
            }

            UpdateCandidatesResponse serverCommand = new UpdateCandidatesResponse
            {
                Candidates = cDTOs.ToArray()
            };

            JsonSerializer serializer = new JsonSerializer();
            string responseJson = serializer.Serialize(serverCommand);
            Console.WriteLine(responseJson);

            await webSocketConnection.SendAsync(responseJson);
        }

        private void OnConnect(WebSocketConnectionServer connection)
        {
            Console.WriteLine($"Connected to {connection}");

            connection.Subscribe(this);
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
                Console.WriteLine("Serwer OTRZYMAŁ DANE O KANDYDATACH od klienta.");
            }
            else if (serializer.GetCommandHeader(message) == MoreInfoCandidateCommand.StaticHeader)
            {
                MoreInfoCandidateCommand moreInfoCandidateCommand = serializer.Deserialize<MoreInfoCandidateCommand>(message);

                CandidateInfoResponse candidateInfoResponse = new CandidateInfoResponse();
                candidateInfoResponse.information = logicAPI.GetCandidateInformation(moreInfoCandidateCommand.Candidate.Value.ID);
                candidateInfoResponse.ID = moreInfoCandidateCommand.Candidate.Value.ID;

                string responseJson = serializer.Serialize(candidateInfoResponse);
                Console.WriteLine(responseJson);

                await webSocketConnection.SendAsync(responseJson);
            }
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

        public void OnCompleted()
        {
            OnClose();
        }

        public void OnError(Exception error)
        {
            OnError();
        }

        public void OnNext(string value)
        {
            OnMessage(value);
        }
    }
}