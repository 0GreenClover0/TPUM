using System.ComponentModel;

namespace Logic
{
    public abstract class IBall
    {
        public static IBall CreateBall(int xPos, int yPos)
        {
            return new Ball(xPos, yPos);
        }

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public abstract int x { get; set; }
        public abstract int y { get; set; }
    }
}
