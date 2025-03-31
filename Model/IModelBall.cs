using System.ComponentModel;
using Logic;

namespace Model
{
    public abstract class IModelBall : INotifyPropertyChanged
    {
        public abstract double X { get; set; }
        public abstract double Y { get; set; }
        public abstract double R { get; set; }

        public static IModelBall CreateModelBall(double x, double y, double r)
        {
            return new ModelBall(x, y, r);
        }

        public abstract void Moving(double x, double y);

        public abstract event PropertyChangedEventHandler? PropertyChanged;
    }
}
