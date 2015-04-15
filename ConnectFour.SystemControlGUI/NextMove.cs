namespace ConnectFour.SystemControlGUI
{
    class NextMove
    {
        public NextMove(int x, int y, int player, int status)
        {
            Status = status;
            Player = player;
            Y = y;
            X = x;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Player { get; private set; }
        public int Status { get; private set; }
    }
}
