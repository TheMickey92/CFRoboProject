using System.Drawing;
using System.Windows.Forms;

namespace ConnectFour.Vision
{
    public partial class ImageView : Form
    {
        public ImageView()
        {
            InitializeComponent();
        }

        public void ShowImage(Bitmap bitmap)
        {
            if (bitmap == null) return;
            Size = bitmap.Size;
            pictureBox.Image = bitmap;
        }
    }
}
