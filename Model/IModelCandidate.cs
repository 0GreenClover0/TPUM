using System.ComponentModel;

namespace Model
{
    public abstract class IModelCandidate : INotifyPropertyChanged
    {
        public static IModelCandidate CreateModelCandidate(int id, string fullName, string party)
        {
            return new ModelCandidate(id, fullName, party);
        }
        public abstract int ID { get; set; }
        public abstract string FullName { get; set; }
        public abstract string Party { get; set; }
        public abstract bool IsChosen { get; set; }

        public abstract event PropertyChangedEventHandler? PropertyChanged;
    }
}
