using System.Drawing;

namespace ConnectFour.Logic
{
    public interface Output
    {
        void SetField(Point field, int currentPlayer);
        void Win(Point field, int currentPlayer);
        void Draw(Point field, int currentPlayer);
    }
}
