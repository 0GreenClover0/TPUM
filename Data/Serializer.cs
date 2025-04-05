using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data
{
    internal class JsonSerializer
    {
        public string Serialize<T>(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public T Deserialize<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }

        public string? GetResponseHeader(string message)
        {
            JObject jObject = JObject.Parse(message);
            if (jObject.ContainsKey("Header"))
            {
                return (string)jObject["Header"];
            }

            return null;
        }
    }
}