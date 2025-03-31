using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    internal class ModelCandidate : IModelCandidate
    {
        public override int ID { get; set; }

        public override string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                NotifyPropertyChanged();
            }
        }

        private string fullName;

        public override string Party
        {
            get { return party; }
            set
            {
                party = value;
                NotifyPropertyChanged();
            }
        }

        private string party;

        public override bool IsChosen
        {
            get { return isChosen; }
            set
            {
                isChosen = value;
                NotifyPropertyChanged();
            }
        }

        private bool isChosen;

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
