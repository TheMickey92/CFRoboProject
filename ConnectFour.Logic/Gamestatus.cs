using System.Drawing;

namespace ConnectFour.Logic
{
    class GameStatus
    {
        public int[,] Field { get; set; }

        public Point[] PossibleMoves { get; set; }

        public int YPos { get; private set; }

        public int ValidMovesCount { get; set; }

        public int CurrentPlayer { get; set; }

        private GameControl gameControl;

        public GameStatus(GameControl gameControl)
        {
            this.gameControl = gameControl;
            Field = new int[7, 6];
            PossibleMoves = new Point[7];
            ValidMovesCount = 7;

            YPos = 5;

            for (int i = 0; i < 7; i++)
            {
                PossibleMoves[i] = new Point(i, 5);
            }
        }

        public void Set(Point p)
        {
            Field[p.X, p.Y] = gameControl.GetCurrentPlayer();
            Point pM = PossibleMoves[p.X];
            if (pM.Y == 0)
            {
                PossibleMoves[p.X] = new Point(-1, -1); // Kein Zug mehr möglich hier!
                ValidMovesCount--;
            }
            else
                PossibleMoves[p.X] = new Point(pM.X, pM.Y - 1);

            // Höhe erhöhen wenn nötig
            if (p.Y < YPos)
                YPos = p.Y;
        }

        public void Set(int x, int y)
        {
            Set(new Point(x, y));
        }

        public void UnSet(int x, int y)
        {
            UnSet(new Point(x, y));
        }

        public void UnSet(Point p)
        {
            Field[p.X, p.Y] = 0;
            if (PossibleMoves[p.X].X == -1)
                ValidMovesCount++;
            PossibleMoves[p.X] = new Point(p.X, p.Y);

            // Höhe herabsetzen, wenn nötig
            if (p.Y == YPos)
            {
                // Prüfen, ob noch ein Feld auf der Höhe vorhanden ist
                for (int i = 0; i < 7; i++)
                {
                    if (PossibleMoves[i].Y == YPos)
                        return;
                }
                YPos++; // ansonsten Höhe herabsetzen 
            }

        }

        public int Get(int x, int y)
        {
            return Get(new Point(x, y));
        }

        public int Get(Point p)
        {
            return Field[p.X, p.Y];
        }

        public bool IsSet(int x, int y)
        {
            return IsSet(new Point(x, y));
        }

        public bool IsSet(Point p)
        {
            return Field[p.X, p.Y] != 0;
        }

        public void ResetPossibleMoves()
        {
            PossibleMoves = new Point[7];
            for (int i = 0; i < 7; i++)
            {
                PossibleMoves[i] = new Point(-1, -1);
            }

            ValidMovesCount = 0;
            YPos = 5;


            for (int x = 0; x < 7; x++)
            {
                for (int y = 5; y >= 0; y--)
                {
                    if (Field[x, y] == 0)
                    {
                        ValidMovesCount++;
                        PossibleMoves[x] = new Point(x, y);
                        if (YPos > y)
                            YPos = y;

                        break;
                    }
                }
            }

            if (YPos > 0)
                YPos -= 1;
        }

        
    }
}
