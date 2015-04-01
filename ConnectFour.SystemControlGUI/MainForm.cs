using System;
using System.IO;
using System.Windows.Forms;

namespace ConnectFour.SystemControlGUI
{
    public partial class MainForm : Form
    {
        private string pathToConsole = "";
        public MainForm()
        {
            InitializeComponent();

            string altPath = "../../../ConnectFour.Console/bin/Debug/CFConsole.exe";

            if (File.Exists("CFConsole.exe"))
                pathToConsole = "CFConsole.exe";
            else if (File.Exists(altPath))
                pathToConsole = altPath;
            else
            {
                MessageBox.Show("CFConsole not found!");
                Environment.Exit(0);
            }
        }
    }
}
