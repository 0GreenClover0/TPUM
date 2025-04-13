namespace ServerData
{
    public abstract class IDashBoard
    {
        public abstract int BoardWidth { get; set; }
        public abstract int BoardHeight { get; set; }

        public static IDashBoard CreateDashBoard(int w, int h)
        {
            return new DashBoard(w, h);
        }

        internal class DashBoard : IDashBoard
        {
            public override int BoardWidth { get; set; }
            public override int BoardHeight { get; set; }

            public DashBoard(int w, int h)
            {
                BoardWidth = w;
                BoardHeight = h;
            }
        }
    }
}
