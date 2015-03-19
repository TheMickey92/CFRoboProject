using System.Collections.Generic;
using System.Drawing;
using ConnectFour.Logic.CatchMoves;

namespace ConnectFour.Logic
{
    public class GameControl
    {
        private IOutput output;
        private IPlayer[] player = new IPlayer[2];
        private Gamestatus gamestatus;
        
        public GameControl(IOutput output)
        {
            this.output = output;
            newGameData();
        }

        private void newGameData()
        {
            gamestatus = new Gamestatus(this);
        }

        public void NewGame(IPlayer player1, IPlayer player2)
        {
            player[0] = player1;
            player[1] = player2;
            newGameData();
            gamestatus.CurrentPlayer = 1;
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
            if(!MoveCheck.IsMoveAllowed(p, gamestatus.Field))
            {
                player[gamestatus.CurrentPlayer - 1].Turn();
                return;
            }

            // Feld setzen, sowohl in den Spieldaten, als auch auf der Oberfläche
            gamestatus.Set(p);
            

            // Sieg überprüfen
            CheckWinResult checkWinResult = CheckWin();
            if (checkWinResult.Winner == gamestatus.CurrentPlayer)
            {
                output.Win(p, gamestatus.CurrentPlayer, checkWinResult.Points);
                return;
            }

            // Unentschieden Prüfen
            if (gamestatus.ValidMovesCount == 0)
            {
                output.Draw(p, gamestatus.CurrentPlayer);
                return;
            }

            output.SetField(p, gamestatus.CurrentPlayer);
            

            // Spieler wechseln
            SwapPlayer();
            player[gamestatus.CurrentPlayer - 1].Turn();
        }
        

        public int[,] GetGamefield()
        {
            int[,] clone = (int[,]) gamestatus.Field.Clone();
            return clone;
        }

        

        public CheckWinResult CheckWin()
        {
            List<Point> points;
            for (int y = gamestatus.YPos; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (x < 4)
                    {
                        // Check X-Axis
                        points = new List<Point>()
                        {
                            new Point(x, y),
                            new Point(x + 1, y),
                            new Point(x + 2, y),
                            new Point(x + 3, y)
                        };
                        if (MoveCheck.IsSetByPlayer(points, gamestatus.CurrentPlayer, gamestatus.Field))
                            return new CheckWinResult(gamestatus.CurrentPlayer, points);

                        if (y > 2)
                        {
                            // Check diagonal upwards
                            points = new List<Point>()
                            {
                                new Point(x, y),
                                new Point(x + 1, y - 1),
                                new Point(x + 2, y - 2),
                                new Point(x + 3, y - 3)
                            };
                            if (MoveCheck.IsSetByPlayer(points, gamestatus.CurrentPlayer, gamestatus.Field))
                                return new CheckWinResult(gamestatus.CurrentPlayer, points);
                        }

                        if (y < 3)
                        {
                            // Check diagonal downwoards
                            points = new List<Point>()
                            {
                                new Point(x, y),
                                new Point(x + 1, y + 1),
                                new Point(x + 2, y + 2),
                                new Point(x + 3, y + 3)
                            };
                            if (MoveCheck.IsSetByPlayer(points, gamestatus.CurrentPlayer, gamestatus.Field))
                                return new CheckWinResult(gamestatus.CurrentPlayer, points);
                        }
                    }

                    if (y < 3)
                    {
                        // Check Y-Axis
                        points = new List<Point>()
                        {
                            new Point(x, y),
                            new Point(x, y + 1),
                            new Point(x, y + 2),
                            new Point(x, y + 3)
                        };
                        if (MoveCheck.IsSetByPlayer(points, gamestatus.CurrentPlayer, gamestatus.Field))
                            return new CheckWinResult(gamestatus.CurrentPlayer, points);
                    }
                }
            }

            return new CheckWinResult(0);
        }


        public Point GetWinPoint(int player)
        {
            return WinPoint.GetWinPoint(player, gamestatus.Field);
        }
        
        
        public Point[] GetPossibleMoves()
        {
            return gamestatus.PossibleMoves;
        }

        public void SwapPlayer()
        {
            if (gamestatus.CurrentPlayer == 1)
            {
                gamestatus.CurrentPlayer = 2;
                return;
            }
            gamestatus.CurrentPlayer = 1;
        }

        

        public void SetGameFieldAndPlayer(int[,] newGameField, int player)
        {
            gamestatus.Field = newGameField;
            gamestatus.CurrentPlayer = player;
        }

        public Point CatchRowTrick()
        {
            return RowTrick.CatchRowTrick(gamestatus.CurrentPlayer, gamestatus.Field);
        }

        public Point UseRowTrick()
        {
            return RowTrick.UseRowTrick(gamestatus.CurrentPlayer, gamestatus.Field);
        }

        public string GamefieldToString()
        {
            string sGameField = "";

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    sGameField += gamestatus.Get(x, y);
                }
            }
            return sGameField;
        }

        public void Set(int x, int y)
        {
            Set(new Point(x, y));
        }

        public void Set(Point move)
        {
            gamestatus.Set(move);
        }

        public void UnSet(Point move)
        {
            gamestatus.UnSet(move);
        }

        public void ResetPossibleMoves()
        {
            gamestatus.ResetPossibleMoves();
        }

        public int GetValidMovesCount()
        {
            return gamestatus.ValidMovesCount;
        }

        public int GetCurrentPlayer()
        {
            return gamestatus.CurrentPlayer;
        }

        public void SetCurrentPlayer(int player)
        {
            gamestatus.CurrentPlayer = player;
        }
    }
}
