using System;
using ConnectFour.FischertechnikInterface;
using ConnectFour.Logic.Strategy;
using ConnectFour.Vision;

namespace ConnectFour.Console
{
    public class ConnectFourConsole
    {
        private static void Main(string[] args)
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

            if (args.Length > 0)
            {
                if(args[0] != "vision" && args[0] != "robot" && args[0] != "memory")
                {
                    handleLogicCall(args);
                }

                if (args[0] == "robot")
                {
                    handleRobotCall(args);
                }

                if (args[0] == "vision")
                {
                    handleVisionCall();
                }

                if (args[0] == "memory")
                {
                    handleMemoryResetCall(args);
                }
            }
            System.Console.WriteLine("-1 -1 -1 -3");
            Environment.Exit(0);
        }

        private static void handleMemoryResetCall(string[] args)
        {
            int deep = 0;
            try
            {
                deep = Convert.ToInt32(args[1]);
            }
            catch (Exception)
            {
                System.Console.WriteLine("-1");
                Environment.Exit(0);
            }
            NegMaxMemorizer negMaxMemorizer = new NegMaxMemorizer(deep);
            negMaxMemorizer.ResetDatabase();
            Environment.Exit(0);
        }

        private static void handleVisionCall()
        {
            VisionControl visionControl = new VisionControl();
            int[,] gamefield = visionControl.Process();
            string json = InputHandling.GetJsonFrom2DArray(gamefield);
            System.Console.WriteLine(json);
            Environment.Exit(0);
        }

        private static void handleRobotCall(string[] args)
        {
            if (args.Length != 2)
            {
                System.Console.WriteLine("-1");
                Environment.Exit(0);
            }

            RobotControl robotControl = new RobotControl();
            bool success = robotControl.PlayMove(Convert.ToInt32(args[1]));
            // Wenn der Spielzug erfolgreich ausgeführt wurde, gib 1 zurück, ansosnten -1
            System.Console.WriteLine(success ? "1" : "-1");
            Environment.Exit(0);
        }

        private static void handleLogicCall(string[] args)
        {
            int length = (args[0] == "logic")?3:2;
            
            if (args.Length != length)
            {
                System.Console.WriteLine("-1 -1 -1 -3");
                Environment.Exit(0);
            }

            LogicProcessor consoleControl = new LogicProcessor();
            if (length == 2)
                consoleControl.Process(args[0], args[1]);
            else
                consoleControl.Process(args[1], args[2]);

            Environment.Exit(0);
        }
    }
}
