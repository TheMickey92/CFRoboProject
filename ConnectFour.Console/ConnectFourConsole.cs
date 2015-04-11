using System;
using AForge.Video.DirectShow;
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
            //oldGameField[0, 5] = 1;
            //oldGameField[1, 5] = 2;
            //oldGameField[2, 5] = 1;
            //oldGameField[3, 5] = 2;
            //oldGameField[4, 5] = 2;
            //oldGameField[5, 5] = 2;
            //oldGameField[6, 5] = 1;

            //oldGameField[0, 4] = 1;
            //oldGameField[1, 4] = 1;
            //oldGameField[2, 4] = 2;
            //oldGameField[3, 4] = 1;
            //oldGameField[4, 4] = 2;
            //oldGameField[5, 4] = 2;
            //oldGameField[6, 4] = 1;

            //oldGameField[0, 3] = 2;
            //oldGameField[1, 3] = 1;
            //oldGameField[2, 3] = 1;
            //oldGameField[3, 3] = 2;
            //oldGameField[4, 3] = 2;
            //oldGameField[5, 3] = 1;
            //oldGameField[6, 3] = 1;

            //oldGameField[0, 2] = 1;
            //oldGameField[1, 2] = 2;
            //oldGameField[2, 2] = 1;
            //oldGameField[3, 2] = 2;
            //oldGameField[4, 2] = 1;
            //oldGameField[5, 2] = 2;
            //oldGameField[6, 2] = 2;

            //oldGameField[0, 1] = 2;
            //oldGameField[1, 1] = 2;
            //oldGameField[2, 1] = 1;
            //oldGameField[3, 1] = 2;
            //oldGameField[4, 1] = 2;
            //oldGameField[5, 1] = 2;
            //oldGameField[6, 1] = 1;
            
            //oldGameField[0, 0] = 0;
            //oldGameField[1, 0] = 0;
            //oldGameField[2, 0] = 2;
            //oldGameField[3, 0] = 1;
            //oldGameField[4, 0] = 2;
            //oldGameField[5, 0] = 1;
            //oldGameField[6, 0] = 1;


            //int[,] newGameField = new int[7, 6];
            //newGameField[0, 5] = 1;
            //newGameField[1, 5] = 2;
            //newGameField[2, 5] = 1;
            //newGameField[3, 5] = 2;
            //newGameField[4, 5] = 2;
            //newGameField[5, 5] = 2;
            //newGameField[6, 5] = 1;

            //newGameField[0, 4] = 1;
            //newGameField[1, 4] = 1;
            //newGameField[2, 4] = 2;
            //newGameField[3, 4] = 1;
            //newGameField[4, 4] = 2;
            //newGameField[5, 4] = 2;
            //newGameField[6, 4] = 1;

            //newGameField[0, 3] = 2;
            //newGameField[1, 3] = 1;
            //newGameField[2, 3] = 1;
            //newGameField[3, 3] = 2;
            //newGameField[4, 3] = 2;
            //newGameField[5, 3] = 1;
            //newGameField[6, 3] = 1;

            //newGameField[0, 2] = 1;
            //newGameField[1, 2] = 2;
            //newGameField[2, 2] = 1;
            //newGameField[3, 2] = 2;
            //newGameField[4, 2] = 1;
            //newGameField[5, 2] = 2;
            //newGameField[6, 2] = 2;

            //newGameField[0, 1] = 2;
            //newGameField[1, 1] = 2;
            //newGameField[2, 1] = 1;
            //newGameField[3, 1] = 2;
            //newGameField[4, 1] = 2;
            //newGameField[5, 1] = 2;
            //newGameField[6, 1] = 1;

            //newGameField[0, 0] = 1;
            //newGameField[1, 0] = 0;
            //newGameField[2, 0] = 2;
            //newGameField[3, 0] = 1;
            //newGameField[4, 0] = 2;
            //newGameField[5, 0] = 1;
            //newGameField[6, 0] = 1;

            //string jsonOldGameField = InputHandling.GetJsonFrom2DArray(oldGameField);
            //string jsonNewGameField = InputHandling.GetJsonFrom2DArray(newGameField);

            //args = new[]
            //{
            //    //"[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,1,1],[0,0,0,0,0,2],[0,0,0,0,0,0],[0,0,0,0,0,0]]",
            //    //"[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,1,1],[0,0,0,2,0,2],[0,0,0,0,0,0],[0,0,0,0,0,0]]"
            //    jsonOldGameField,
            //    jsonNewGameField
            //};

            //args = new[] { "vision" };

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
                    handleVisionCall(args);
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

        private static void handleVisionCall(string[] args)
        {
            int device = 0;
            VisionControl visionControl = new VisionControl();

            if (args.Length > 2)
            {
                System.Console.Out.WriteLine("-1");
                Environment.Exit(0);
            }

            if (args.Length == 2)
            {
                if (args[1] == "devices")
                {
                    FilterInfoCollection devices = visionControl.GetDevices();
                    string sDevices = "";
                    foreach (FilterInfo filterInfo in devices)
                    {
                        sDevices = filterInfo.Name + "; ";
                    }
                    System.Console.Out.WriteLine(sDevices);
                    Environment.Exit(0);
                }

                try
                {
                    device = Convert.ToInt32(args[1]);
                }
                catch (Exception)
                {
                    System.Console.Out.WriteLine("-1");
                    Environment.Exit(0);
                }
            }

            
            int[,] gamefield = visionControl.Process(device);
            string json = InputHandling.GetJsonFrom2DArray(gamefield);
            System.Console.WriteLine(json);
            Environment.Exit(0);
        }

        private static void handleRobotCall(string[] args)
        {
            if (args.Length != 2 && args.Length != 3)
            {
                System.Console.WriteLine("-1");
                Environment.Exit(0);
            }
            
            RobotControl robotControl;
            if (args.Length == 3)
                robotControl = new RobotControl(args[2]);
            else // wenn keine IP übergeben wurde, dann wird Standard USB IP genutzt
                robotControl = new RobotControl("192.168.7.2");


            RoboticsErrorCode errorCode = robotControl.PlayMove(Convert.ToInt32(args[1]));
            // Wenn der Spielzug erfolgreich ausgeführt wurde, gib 1 zurück, ansosnten -1

            switch (errorCode)
            {
                case RoboticsErrorCode.OK:
                    System.Console.WriteLine("1");
                    break;
                case RoboticsErrorCode.NOT_CONNECTED:
                    System.Console.WriteLine("-2");
                    break;
                case RoboticsErrorCode.TIMEOUT:
                    System.Console.WriteLine("-3");
                    break;
                case RoboticsErrorCode.UNDEFINED:
                    System.Console.WriteLine("-1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Environment.Exit(0);
        }

        private static void handleLogicCall(string[] args)
        {
            int length = (args[0] == "logic") ? 3 : 2;
            
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
