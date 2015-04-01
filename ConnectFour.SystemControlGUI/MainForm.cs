using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ConnectFour.SystemControlGUI
{
    public partial class MainForm : Form
    {
        private string pathToConsole = "";
        private string lastJSONStatus;
        private Process console;
        private string ip = "";

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

            console = new Process();
            console.StartInfo.FileName = pathToConsole;
            console.StartInfo.UseShellExecute = false;
            console.StartInfo.RedirectStandardOutput = true;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mainTimer.Enabled = false;
            secondChanceTimer.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            mainTimer.Interval = 5000;
            mainTimer.Enabled = true;
            lastJSONStatus =
                "[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0]]";

            ip = String.IsNullOrWhiteSpace(txtIP.Text) ? "" : txtIP.Text;
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            mainTimer.Enabled = false;

            string newJSONStatus = processVision();

            string nextMoveX = processNextMove(lastJSONStatus, newJSONStatus);
            if (nextMoveX == "-1")
            {
                MessageBox.Show("GAME OVER!");
                return;
            }

            // TODO: Gewinner/Unentschieden anzeigen, SecondChance bei vermeintluchem Schummeln 

            processRobotics(nextMoveX, ip);


            mainTimer.Enabled = true;
        }


        private string processConsoleTest()
        {
            console.StartInfo.Arguments = "";
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        private string processVision()
        {
            console.StartInfo.Arguments = "vision";
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        private string processNextMove(string oldField, string newField)
        {
            return "";
        }

        private string processRobotics(string x, string ip="")
        {
            console.StartInfo.Arguments = "robot " + x + ((ip == "") ? "" : " " + ip);
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        
    }
}
