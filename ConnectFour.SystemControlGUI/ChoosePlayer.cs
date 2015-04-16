using System;
using System.Windows.Forms;

namespace ConnectFour.SystemControlGUI
{
    public partial class ChoosePlayer : Form
    {
        public ChoosePlayer()
        {
            InitializeComponent();
            Player = 1;
        }

        public int Player { get; private set; }

        private void numPlayer_ValueChanged(object sender, EventArgs e)
        {
            Player = (int) numPlayer.Value;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
