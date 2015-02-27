using System.Drawing;

namespace ConnectFour.Vision
{
    public class VisionControl
    {
        private string path = @"C:\Users\MarcelB\Documents\Visual Studio 2013\Projects\CFRoboProject\testpicture.jpg"; // TODO anstatt über Pfad, macht die Kamera ein Bild
        public int[,] Process()
        {
            Bitmap bitmap = new Bitmap(path);
            FieldMap fieldMap = new FieldMap();
            int[,] gamefield = new int[7,6];
            
            // TODO Try Catch!

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    Point position = fieldMap.GetPosition(x, y);
                    Color color = bitmap.GetPixel(position.X, position.Y);
                    if (color.R > 150 && color.G > 150 && color.B < 100) // gelb
                        gamefield[x, y] = 1;
                    else if(color.R > 200) // rot
                        gamefield[x, y] = 2;
                }
            }

            return gamefield;
        }
    }
}
