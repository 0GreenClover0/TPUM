using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    internal class ModelBall : IModelBall
    {
        private double x;
        private double y;

        public override double R { get; set; }
        public override double X
        {
            get { return x; }
            set
            {
                x = value;
                NotifyPropertyChanged();
            }
        }
        public override double Y
        {
            get { return y; }
            set
            {
                y = value;
                NotifyPropertyChanged();
            }
        }

        public ModelBall(double x, double y, double r)
        {
            this.x = x;
            this.y = y;
            this.R = r;
        }

        public override void Moving(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
