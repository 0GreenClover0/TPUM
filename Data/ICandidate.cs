using System.Runtime.Serialization;

namespace Data
{
    public abstract class ICandidate : ISerializable
    {
        public abstract int ID { get; }
        public abstract string FullName { get; }
        public abstract string Party { get; }
        public abstract bool IsChosen { get; set; }
        public abstract event EventHandler<Event>? OnPropertyChanged;
        public abstract void ChooseCandidate();
        public abstract void DeselectCandidate();

        public static ICandidate CreateCandidate(int id, string name, string party)
        {
            return new Candidate(id, name, party);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID: ", ID);
            info.AddValue("FullName: ", FullName);
            info.AddValue("Party: ", Party);
        }
    }
}
