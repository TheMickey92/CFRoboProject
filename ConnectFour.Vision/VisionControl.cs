using System;
using System.Drawing;
using System.Threading;
using AForge.Video;
using AForge.Video.DirectShow;

namespace ConnectFour.Vision
{
    public class VisionControl
    {
        private string path = @"C:\Users\MarcelB\Documents\Visual Studio 2013\Projects\CFRoboProject\testpicture2.jpg"; // TODO anstatt über Pfad, macht die Kamera ein Bild
        private VideoCaptureDevice camera;
        private Bitmap tempBitmap;

        public int[,] Process(int device)
        {
            //Bitmap bitmap = new Bitmap(path);
            Bitmap bitmap = getImage(device);
            if (bitmap == null)
            {
                Console.WriteLine("-1");
                Environment.Exit(0);
            }

            FieldMap fieldMap = new FieldMap();
            int[,] gamefield = new int[7,6];
            
            // TODO Try Catch!

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    Point position = fieldMap.GetPosition(x, y);
                    Color color = bitmap.GetPixel(position.X, position.Y);
                    if (color.R > 150 && color.G > 150) // gelb
                        gamefield[x, y] = 1;
                    else if(color.R > 150) // rot
                        gamefield[x, y] = 2;
                }
            }

            return gamefield;
        }

        public FilterInfoCollection GetDevices()
        {
            return new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        private Thread cameraThread;
        private Bitmap getImage(int device)
        {
            try
            {
                FilterInfoCollection devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                camera = new VideoCaptureDevice(devices[device].MonikerString);
                camera.NewFrame += newFrameEventArgs;
                cameraThread = new Thread(cameraAction);
                cameraThread.Start();
                while (cameraThread.IsAlive) { }
                return tempBitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool pictureTaken;

        private void cameraAction()
        {
            camera.Start();
            pictureTaken = false;
            while (!pictureTaken) { }
            camera.Stop();
            cameraThread.Abort();
        }

        private void newFrameEventArgs(object sender, NewFrameEventArgs eventArgs)
        {
            tempBitmap = (Bitmap) eventArgs.Frame.Clone();
            pictureTaken = true;
        }

        public void ShowCurrentView(int device)
        {
            Bitmap bitmap = getImage(device);
            ImageView imageView = new ImageView();
            imageView.ShowImage(bitmap);
            imageView.ShowDialog();
        }
    }
}
