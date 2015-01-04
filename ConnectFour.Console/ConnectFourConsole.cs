using System;

namespace ConnectFour.Console
{
    public class ConnectFourConsole
    {
        static void Main(string[] args)
        {
            //int[,] oldGameField = new int[7, 6];
            //oldGameField[3, 5] = 2;
            //oldGameField[4, 5] = 1;
            //oldGameField[3, 4] = 2;
            //oldGameField[4, 4] = 1;

            //int[,] newGameField = new int[7, 6];
            //newGameField[3, 5] = 2;
            //newGameField[4, 5] = 1;
            //newGameField[3, 4] = 2;
            //newGameField[4, 4] = 1;
            //newGameField[4, 3] = 1;

            //string jsonOldGameField = InputHandling.GetJsonFrom2DArray(oldGameField);
            //string jsonNewGameField = InputHandling.GetJsonFrom2DArray(newGameField);

            //args = new[]
            //{
            //    //"[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,1,1],[0,0,0,0,0,2],[0,0,0,0,0,0],[0,0,0,0,0,0]]",
            //    //"[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,1,1],[0,0,0,2,0,2],[0,0,0,0,0,0],[0,0,0,0,0,0]]"
            //    jsonOldGameField,
            //    jsonNewGameField
            //};

            if (args.Length != 2)
            {
                System.Console.WriteLine("-1 -1 -1 -3");
                Environment.Exit(0);
            }

            ConsoleControl consoleControl = new ConsoleControl();
            consoleControl.Process(args[0], args[1]);
        }
    }
}
