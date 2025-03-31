using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    internal class ModelCandidate : IModelCandidate
    {
        public override int ID { get; set; }

        public override string FullName
        {
            get { return FullName; }
            set
            {
                FullName = value;
                NotifyPropertyChanged();
            }
        }

        public override string Party
        {
            get { return Party; }
            set
            {
                Party = value;
                NotifyPropertyChanged();
            }
        }

        public override bool IsChosen
        {
            get { return IsChosen; }
            set
            {
                IsChosen = value;
                NotifyPropertyChanged();
            }
        }

        public ModelCandidate(int id, string fullName, string party)
        {
            ID = id;
            FullName = fullName;
            Party = party;
            IsChosen = false;
        }

        public override event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
