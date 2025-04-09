using System.Text;

namespace ClientAPI
{
    public static class Utilities
    {
        public static ArraySegment<byte> StringToBytes(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(buffer);
        }
    }

    [Serializable]
    public abstract class ServerCommand
    {
        public static readonly string ClosedConnectionHeader = "Closed";
        public string Header;

        protected ServerCommand(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public abstract class ServerResponse
    {
        public string Header { get; private set; }

        protected ServerResponse(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public class ChooseCandidateCommand : ServerCommand
    {
        public static readonly string StaticHeader = "UpdateCandidates";

        public CandidateDTO[]? Candidates;

        public ChooseCandidateCommand()
            : base(StaticHeader)
        {
        }
    }

    [Serializable]
    public struct CandidateDTO
    {
        public int ID;
        public string FullName;
        public string Party;
        public bool IsChosen;

        public CandidateDTO(int id, string name, string party, bool isChosen)
        {
            ID = id;
            FullName = name;
            Party = party;
            IsChosen = isChosen;
        }
    }

    [Serializable]
    public class UpdateCandidatesResponse : ServerResponse
    {
        public static readonly string StaticHeader = "UpdateCandidates";

        public CandidateDTO[]? Candidates;

        public UpdateCandidatesResponse()
            : base(StaticHeader)
        {
        }
    }

    [Serializable]
    public class TimerResponse : ServerResponse
    {
        public static readonly string StaticHeader = "TimerChanged";

        public int NewTime;

        public TimerResponse()
            : base(StaticHeader)
        {
        }
    }
}
