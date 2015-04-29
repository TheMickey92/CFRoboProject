using System.Threading;
using MCD.Robots;

namespace ConnectFour.FischertechnikInterface
{
    public class RobotControl
    {
        private FTConnectionInterface ftci;
        private const int dropMax = 1580;

        private static readonly int[] fieldPositions = {5121, 4540, 3970, 3412, 2845, 2277, 1718};
        private const int EPSILON = -50;

        public RobotControl(string ip)
        {
            ftci = new FTConnectionInterface(ip);
        }


        public RoboticsErrorCode PlayMove(int x)
        {
            RoboticsErrorCode errorCode;


            //while (true)
            //{
                // CONNECT
                if (!ftci.Connected()) return RoboticsErrorCode.NOT_CONNECTED;

                //InterfaceInformationPacket iip = ftci.GetInterfaceInformation();
                //for (int i = 0; i < 8; i++)
                //{
                //    bool b = iip.I(i);
                //    Console.WriteLine("b" + i + ": " + (b ? 1 : 0));
                //}

                // DISCONNECT
                //iip = ftci.GetInterfaceInformation();
                ftci.Disconnect();

            //    Thread.Sleep(1000);
            //}

            

            // GO HOME
            errorCode = goHome();
            if (errorCode != RoboticsErrorCode.OK)
                return errorCode;

            errorCode = play(x);
            if (errorCode != RoboticsErrorCode.OK)
                return errorCode;


            // GO HOME AGAIN
            errorCode = goHome();
            if (errorCode != RoboticsErrorCode.OK)
                return errorCode;


            
            return RoboticsErrorCode.OK;
        }

        private RoboticsErrorCode goHome()
        {
            // CONNECT
            if (!ftci.Connected()) return RoboticsErrorCode.NOT_CONNECTED;

            InterfaceInformationPacket iip = ftci.GetInterfaceInformation();

            for (int i = 0; i < 50; i++)
            {
                iip = ftci.SendInterfacePacket(MotorMovement.RIGHT, MotorMovement.STOP, MotorMovement.STOP,
                    MotorMovement.STOP);
            }

            iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP,
                MotorMovement.STOP,
                true, true, true, true); // RESET der Incrementalgeber und Motoren

            iip = ftci.GetInterfaceInformation();

            while (iip.C(2) == 0) // Solange bis Schalter erreicht wurde oder Zeit abgelaufen ist
            {
                iip = ftci.SendInterfacePacket(MotorMovement.LEFT, MotorMovement.STOP, MotorMovement.STOP,
                    MotorMovement.STOP);
                //iip = ftci.GetInterfaceInformation();
                //Console.WriteLine(iip.C(3));
            }

            //iip = ftci.GetInterfaceInformation();
            //while (!iip.I(1) && !(elapsedMilliseconds > 90000))
            //{
            //    iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.LEFT, MotorMovement.STOP, MotorMovement.STOP);
            //    elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            //}

            iip = ftci.GetInterfaceInformation();

            iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP,
                MotorMovement.STOP,
                true, true, true, true); // RESET der Incrementalgeber und Motoren
            iip = ftci.GetInterfaceInformation();

            // DISCONNECT
            ftci.Disconnect();
            return RoboticsErrorCode.OK;
        }

        private RoboticsErrorCode play(int x)
        {
            // CONNECT
            if (!ftci.Connected()) return RoboticsErrorCode.NOT_CONNECTED;

            int neededC = fieldPositions[x] + EPSILON;

            InterfaceInformationPacket iip = ftci.GetInterfaceInformation();
            int c = iip.C(0);

            // Fahre zu Position
            while (c < neededC) // Solange Position noch nicht erreicht wurde
            {
                iip = ftci.SendInterfacePacket(MotorMovement.RIGHT, MotorMovement.STOP, MotorMovement.STOP,
                    MotorMovement.STOP);

                //iip = ftci.GetInterfaceInformation();
                c = iip.C(0);
            }

            // DISCONNECT
            ftci.Disconnect();

            // Lass Stein fallen
            return drop();

        }

        private RoboticsErrorCode drop()
        {
            // CONNECT
            if (!ftci.Connected()) return RoboticsErrorCode.NOT_CONNECTED;

            InterfaceInformationPacket iip = ftci.GetInterfaceInformation();
            iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP,
                MotorMovement.STOP,
                true, true, true, true); // RESET der Incrementalgeber und Motoren


            //for (int i = 0; i < 50; i++)
            //{
            //    iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.RIGHT, MotorMovement.STOP,
            //        MotorMovement.STOP);
            //}



            iip = ftci.GetInterfaceInformation();

            while (iip.C(3) == 0)
            {
                iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.LEFT, MotorMovement.STOP,
                    MotorMovement.STOP);
            }

            iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP,
                MotorMovement.STOP,
                true, true, true, true); // RESET der Incrementalgeber und Motoren


            iip = ftci.GetInterfaceInformation();
            int c = iip.C(1);
            while (c < dropMax)
            {
                iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.RIGHT, MotorMovement.STOP,
                    MotorMovement.STOP);
                c = iip.C(1);
            }


            iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP,
                MotorMovement.STOP,
                true, true, true, true); // RESET der Incrementalgeber und Motoren

            //iip = ftci.GetInterfaceInformation();

            //while (iip.C(3) == 0)
            //{
            //    iip = ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.LEFT, MotorMovement.STOP,
            //        MotorMovement.STOP);
            //}

            Thread.Sleep(500);

            // DISCONNECT
            ftci.Disconnect();

            return RoboticsErrorCode.OK;
        }

        public RoboticsErrorCode TurnOnLED(string color, int length = 20)
        {
            // CONNECT
            if (!ftci.Connected()) return RoboticsErrorCode.NOT_CONNECTED;

            for (int i = 0; i < length; i++)
            {
                if (color == "red" || color == "RED")
                {
                    ftci.SendInterfacePacket(MotorMovement.STOP, 0, MotorMovement.STOP, 0, MotorMovement.STOP, 0,
                        MotorMovement.LEFT, 250);
                }
                else if (color == "green" || color == "GREEN")
                {
                    ftci.SendInterfacePacket(MotorMovement.STOP, 0, MotorMovement.STOP, 0, MotorMovement.LEFT, 250,
                        MotorMovement.STOP, 0);
                }
                else
                {
                    return RoboticsErrorCode.UNDEFINED;
                }
            }


            // DISCONNECT
            ftci.Disconnect();

            return RoboticsErrorCode.OK;
        }
    }
}
