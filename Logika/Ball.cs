using Dane;
using System.ComponentModel;

namespace Logic
{
    internal class Ball : IBall, INotifyPropertyChanged
    {
        internal Ball(int x, int y)
        {
            this.x = x; this.y = y;
        }

        private int X;
        private int Y;

        public override int x
        {
            get { return this.X; }
            set
            {
                this.X = value;
                OnPropertyChanged(nameof(x));
            }
        }

        public override int y
        {
            get { return this.Y; }
            set
            {
                this.Y = value;
                OnPropertyChanged(nameof(y));
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly DataApi dataApi = DataApi.CreateNewInstance();

    }
}