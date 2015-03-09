using System;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class ChoosePlayer : Form
    {
        public bool WasOkClicked { get; private set; }

        public int Player { get; private set; }

        public ChoosePlayer()
        {
            InitializeComponent();
            WasOkClicked = false;
            Player = 0;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            WasOkClicked = true;
            Close();
        }

        private void numPlayer_ValueChanged(object sender, EventArgs e)
        {
            Player = (int) numPlayer.Value;
        }
    }
}
