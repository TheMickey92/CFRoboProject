using System.Drawing;

namespace ConnectFour.Console
{
    public class ProcessedInformation
    {
        private InputState inputState;
        private Point playedMove;

        public InputState InputState
        {
            get { return inputState; }
            set { inputState = value; }
        }

        public Point PlayedMove
        {
            get { return playedMove; }
            set { playedMove = value; }
        }

        public ProcessedInformation(InputState inputState, Point playedMove)
        {
            InputState = inputState;
            PlayedMove = playedMove;
        }
    }
}
