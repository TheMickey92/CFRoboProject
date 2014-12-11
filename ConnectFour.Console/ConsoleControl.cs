using System;
using System.Drawing;
using ConnectFour.Logic;

namespace ConnectFour.Console
{
    public class ConsoleControl : IOutput, IPlayer
    {
        private int player;
        public void Process(int[,] oldGameField, int[,] newGameField)
        {
            ProcessedInformation processedInformation = InputHandling.ProcessInput(oldGameField, newGameField);

            switch (processedInformation.InputState)
            {
                case InputState.PLAYER1:
                    player = 1;
                    break;
                case InputState.PLAYER2:
                    player = 2;
                    break;
                case InputState.CHEAT:
                    System.Console.WriteLine("-1 -1 -1 -2");
                    Environment.Exit(0);
                    return;
                case InputState.FORMATERROR:
                    System.Console.WriteLine("-1 -1 -1 -3");
                    Environment.Exit(-10);
                    return;
                case InputState.UNDEFINED:
                    System.Console.WriteLine("-1 -1 -1 -4");
                    Environment.Exit(-11);
                    return;
                case InputState.NOCHANGE:
                    System.Console.WriteLine("-1 -1 -1 -5");
                    Environment.Exit(0-12);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GameControl gameControl = new GameControl(this);
            if (player == 1)
                gameControl.SetPlayer(this, new NegMaxPlayer(gameControl));
            else
                gameControl.SetPlayer(new NegMaxPlayer(gameControl), this);

            gameControl.SetGameFieldAndPlayer(oldGameField, player);
            gameControl.ResetPossibleMoves();
            gameControl.Move(processedInformation.PlayedMove);
        }

        public void Process(string oldGameField, string newGameField)
        {
            int[,] aOldGameField = InputHandling.Get2DArrayFromJSON(oldGameField);
            int[,] aNewGameField = InputHandling.Get2DArrayFromJSON(newGameField);

            Process(aOldGameField, aNewGameField);
        }

        public void SetField(Point field, int currentPlayer)
        {
            if (currentPlayer == player) return;
            System.Console.WriteLine(field.X + " " + field.Y + " " + (currentPlayer==1?2:1) + " -1");
            Environment.Exit(0);
        }

        public void Win(Point field, int currentPlayer)
        {
            System.Console.WriteLine(field.X + " " + field.Y + " " + currentPlayer + " " + currentPlayer);
            Environment.Exit(0);
        }

        public void Draw(Point field, int currentPlayer)
        {
            System.Console.WriteLine(field.X + " " + field.Y + " " + currentPlayer + " 0");
            Environment.Exit(0);
        }

        public void Turn()
        {
            System.Console.WriteLine("-1 -1 -1 -4");
            Environment.Exit(0);
        }
    }
}
