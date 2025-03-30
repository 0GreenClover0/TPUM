using Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    internal class ModelBall : IModelBall, INotifyPropertyChanged
    {
        internal ModelBall(IBall ball)
        {
            ball.PropertyChanged += propertyChanged;
            this.x = ball.x;
            this.y = ball.y;
        }

        private int X;
        private int Y;

        public override int x
        {
            get { return X; }
            set
            {
                X = value;
                OnPropertyChanged("x");
            }
        }
        public override int y
        {
            get { return Y; }
            set
            {
                Y = value;
                OnPropertyChanged("y");
            }
        }

        private void propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IBall ball = (IBall)sender;
            if (e.PropertyName == "x")
            {
                this.x = ball.x;
            }
            if (e.PropertyName == "y")
            {
                this.y = ball.y;
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
