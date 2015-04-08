using System.Drawing;
using System.Windows.Forms;

namespace ConnectFour.SystemControlGUI
{
    public partial class FieldView : Form
    {
        private Panel[,] panels = new Panel[7,6];
        public FieldView()
        {
            InitializeComponent();
            initializePanels();
        }

        private void initializePanels()
        {
            int size = 50;
            Size = new Size(7*size + 70, 6*size + 60);
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    
                    int xPos = x*size + 10;
                    int yPos = y*size + 10;

                    Panel panel = new Panel();
                    panel.Location = new Point(xPos, yPos);
                    panel.Size = new Size(size, size);
                    panel.BackColor = Color.Black;
                    Controls.Add(panel);
                    panels[x, y] = panel;
                }
            }
        }

        public void SetField(int[,] gamefield)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if(gamefield[x,y] == 1)
                        panels[x,y].BackColor = Color.Yellow;
                    else if (gamefield[x, y] == 2)
                        panels[x, y].BackColor = Color.Red;
                    else
                        panels[x, y].BackColor = Color.Black;
                }
            }
        }

    }
}
