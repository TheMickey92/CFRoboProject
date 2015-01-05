using System.Collections.Generic;
using System.Drawing;
using ConnectFour.Logic.CatchMoves;

namespace ConnectFour.Logic
{
    public class GameControl
    {
        private IOutput output;
        private IPlayer[] player = new IPlayer[2];
        private int[,] gamefield;
        private Point[] possibleMoves = new Point[7];
        private int yPos = 5;

        public int ValidMovesCount { get; set; }
        public int CurrentPlayer { get; set; }

        public GameControl(IOutput output)
        {
            this.output = output;
            newGameData();
        }

        private void newGameData()
        {
            gamefield = new int[7, 6];
            for (int i = 0; i < 7; i++)
            {
                possibleMoves[i] = new Point(i, 5);
            }

            ValidMovesCount = 7;
            yPos = 5;
        }

        public void NewGame(IPlayer player1, IPlayer player2)
        {
            player[0] = player1;
            player[1] = player2;
            newGameData();
            CurrentPlayer = 1;
            player[0].Turn();
        }

        public void SetPlayer(IPlayer player1, IPlayer player2)
        {
            player[0] = player1;
            player[1] = player2;
        }

        public void Move(int x, int y)
        {
            Move(new Point(x, y));
        }
        
        public void Move(Point p)
        {
            // darf Feld überhaupt gesetzt werden?
            if (IsSet(p) || (p.Y <= 4 && !IsSet(p.X, p.Y + 1)))
            {
                player[CurrentPlayer-1].Turn();
                return;
            }

            // Feld setzen, sowohl in den Spieldaten, als auch auf der Oberfläche
            Set(p);
            

            // Sieg überprüfen
            if (CheckWin() == CurrentPlayer)
            {
                output.Win(p, CurrentPlayer);
                return;
            }

            // Unentschieden Prüfen
            if (ValidMovesCount == 0)
            {
                output.Draw(p, CurrentPlayer);
                return;
            }

            output.SetField(p, CurrentPlayer);
            

            // Spieler wechseln
            SwapPlayer();
            player[CurrentPlayer - 1].Turn();
        }
        
        public void Set(int x, int y)
        {
            Set(new Point(x, y));
        }

        public void Set(Point p)
        {
            gamefield[p.X, p.Y] = CurrentPlayer;
            Point pM = possibleMoves[p.X];
            if (pM.Y == 0)
            {
                possibleMoves[p.X] = new Point(-1, -1); // Kein Zug mehr möglich hier!
                ValidMovesCount--;
            }
            else
                possibleMoves[p.X] = new Point(pM.X, pM.Y - 1);

            // Höhe erhöhen wenn nötig
            if (p.Y < yPos)
                yPos = p.Y;
        }

        public void UnSet(int x, int y)
        {
            UnSet(new Point(x, y));
        }

        public void UnSet(Point p)
        {
            gamefield[p.X, p.Y] = 0;
            if (possibleMoves[p.X].X == -1)
                ValidMovesCount++;
            possibleMoves[p.X] = new Point(p.X, p.Y);

            // Höhe herabsetzen, wenn nötig
            if (p.Y == yPos)
            {
                // Prüfen, ob noch ein Feld auf der Höhe vorhanden ist
                for (int i = 0; i < 7; i++)
                {
                    if (possibleMoves[i].Y == yPos)
                        return;
                }
                yPos++; // ansonsten Höhe herabsetzen 
            }
        }

        public int Get(int x, int y)
        {
            return Get(new Point(x, y));
        }

        public int Get(Point p)
        {
            return gamefield[p.X, p.Y];
        }

        public bool IsSet(int x, int y)
        {
            return IsSet(new Point(x, y));
        }

        public bool IsSet(Point p)
        {
            return gamefield[p.X, p.Y] != 0;
        }

        public int CheckWin()
        {
            for (int y = yPos; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                        if (x < 4)
                        {
                            // Check X-Axis
                            if (MoveCheck.IsSetByPlayer(new List<Point>() { new Point(x, y), new Point(x + 1, y), new Point(x + 2, y), new Point(x + 3, y) }, CurrentPlayer, gamefield))
                                return CurrentPlayer;

                            if (y > 2)
                            {
                                // Check diagonal upwards
                                if (MoveCheck.IsSetByPlayer(new List<Point>() { new Point(x, y), new Point(x + 1, y - 1), new Point(x + 2, y - 2), new Point(x + 3, y - 3) }, CurrentPlayer, gamefield))
                                    return CurrentPlayer;
                            }

                            if (y < 3)
                            {
                                // Check diagonal downwoards
                                if (MoveCheck.IsSetByPlayer(new List<Point>() { new Point(x, y), new Point(x + 1, y + 1), new Point(x + 2, y + 2), new Point(x + 3, y + 3) }, CurrentPlayer, gamefield))
                                    return CurrentPlayer;
                            }
                        }

                        if (y < 3)
                        {
                            // Check Y-Axis
                            if (MoveCheck.IsSetByPlayer(new List<Point>() { new Point(x, y), new Point(x, y + 1), new Point(x, y + 2), new Point(x, y + 3) }, CurrentPlayer, gamefield))
                                return CurrentPlayer;
                        }
                    }
                }
            


            return 0;
        }


        public Point GetWinPoint(int player)
        {
            return WinPoint.GetWinPoint(player, yPos, gamefield);
        }
        
        
        public Point[] GetPossibleMoves()
        {
            return possibleMoves;
        }

        public void SwapPlayer()
        {
            if (CurrentPlayer == 1)
            {
                CurrentPlayer = 2;
                return;
            }
            CurrentPlayer = 1;
        }

        public void ResetPossibleMoves()
        {
            possibleMoves = new Point[7];
            for (int i = 0; i < 7; i++)
            {
                possibleMoves[i] = new Point(-1, -1);
            }

            ValidMovesCount = 0;
            yPos = 5;
            

            for (int x = 0; x < 7; x++)
            {
                for (int y = 5; y >= 0; y--)
                {
                    if (gamefield[x, y] == 0)
                    {
                        ValidMovesCount++;
                        possibleMoves[x] = new Point(x, y);
                        if (yPos > y)
                            yPos = y;

                        break;
                    }
                }
            }

            if (yPos > 0)
                yPos -= 1;
        }

        public void SetGameFieldAndPlayer(int[,] newGameField, int player)
        {
            gamefield = newGameField;
            CurrentPlayer = player;
        }

        public Point CatchRowTrick()
        {
            return RowTrick.CatchRowTrick(CurrentPlayer, gamefield);
        }

        public Point UseRowTrick()
        {
            return RowTrick.UseRowTrick(CurrentPlayer, gamefield);
        }
    }
}
