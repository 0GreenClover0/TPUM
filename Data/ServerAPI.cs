using Newtonsoft.Json;
using System.CodeDom.Compiler;

namespace ConnectionAPI
{
    internal static class ServerStatics
    {
        public static readonly string ClosedConnection = "ClosedConnection";
        public static readonly string UpdateCandidates = "UpdateCandidates";
        public static readonly string MoreInfoCandidate = "MoreInfoCandidate";
        public static readonly string TimerChanged = "TimerChanged";
        public static readonly string CandidateInfo = "CandidateInfo";
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal abstract class ServerCommand
    {
        [JsonProperty("Header", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Header { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public abstract class ServerResponse
    {
        [JsonProperty("Header", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Header { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal class ChooseCandidateCommand : ServerCommand
    {
        [JsonProperty("Candidates", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<CandidateDTO> Candidates { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal class MoreInfoCandidateCommand : ServerCommand
    {
        [JsonProperty("Candidate", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public CandidateDTO Candidate { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal class CandidateDTO
    {
        [JsonProperty("ID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }

        [JsonProperty("FullName", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        [JsonProperty("Party", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Party { get; set; }

        [JsonProperty("IsChosen", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool IsChosen { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal class UpdateCandidatesResponse : ServerResponse
    {
        [JsonProperty("Candidates", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<CandidateDTO> Candidates { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal class TimerResponse : ServerResponse
    {
        [JsonProperty("NewTime", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int NewTime { get; set; }
    }

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal class CandidateInfoResponse : ServerResponse
    {
        [JsonProperty("Information", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Information { get; set; }

        [JsonProperty("ID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }
    }
}
