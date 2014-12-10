using System.Drawing;

namespace ConnectFour.Logic
{
    public interface IOutput
    {
        void SetField(Point field, int currentPlayer);
        void Win(Point field, int currentPlayer);
        void Draw(Point field, int currentPlayer);
    }
}
