using System;
using System.Diagnostics;
using MCD.Robots;

namespace ConnectFour.FischertechnikInterface
{
    public class RobotControl
    {
        private FTConnectionInterface ftci;
        private const int lastFieldIncValue = 1300;


        public RobotControl(string ip)
        {
            ftci = new FTConnectionInterface(ip);
        }


        public RoboticsErrorCode PlayMove(int x)
        {
            RoboticsErrorCode errorCode;
            
            // CONNECT
            if(!ftci.Connected()) return RoboticsErrorCode.NOT_CONNECTED;

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


            // DISCONNECT
            ftci.Disconnect();
            return RoboticsErrorCode.OK;
        }

        private RoboticsErrorCode goHome()
        {
            InterfaceInformationPacket iip = ftci.GetInterfaceInformation();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long elapsedMilliseconds = 0;
            while (!iip.I(0) && !(elapsedMilliseconds > 40000)) // Solange bis Schalter erreicht wurde oder Zeit abgelaufen ist
            {
                iip = ftci.SendInterfacePacket(MotorMovement.LEFT, MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP);
                elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            }

            iip = ftci.GetInterfaceInformation();
            if (iip.I(0))
            {
                ftci.SendInterfacePacket(MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP, MotorMovement.STOP,
                    true, true, true, true); // RESET der Incrementalgeber und Motoren
                return RoboticsErrorCode.OK;
            }
            return RoboticsErrorCode.TIMEOUT;
        }

        private RoboticsErrorCode play(int x)
        {
            int neededC = (int) Math.Round(lastFieldIncValue/7.0*x, MidpointRounding.AwayFromZero);

            InterfaceInformationPacket iip = ftci.GetInterfaceInformation();

            // Fahre zu Position
            while (iip.C(0) < neededC) // Solange Position noch nicht erreicht wurde
            {
                iip = ftci.SendInterfacePacket(MotorMovement.RIGHT, MotorMovement.STOP, MotorMovement.STOP,
                    MotorMovement.STOP);
            }

            // Lass Stein fallen


            return RoboticsErrorCode.OK;
        }
    }
}
