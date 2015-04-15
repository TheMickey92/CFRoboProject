using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ConnectFour.Console;

namespace ConnectFour.SystemControlGUI
{
    public partial class MainForm : Form
    {
        private string pathToConsole = "";
        private string lastJSONStatus;
        private Process console;
        private string ip = "";
        FieldView fieldView = new FieldView();

        private bool cheat;

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
            lastJSONStatus =
                "[[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0]]";

            ip = String.IsNullOrWhiteSpace(txtIP.Text) ? "" : txtIP.Text;

            cheat = false;

            mainTimer.Interval = 5000;
            mainTimer.Enabled = true;

            mainTimer_Tick(sender, e);
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            mainTimer.Enabled = false;

            // Bilderkennung
            string newJSONStatus = imageRecognition();
            if (newJSONStatus == "-1")
            {
                MessageBox.Show(
                    "Bei der Bilderkennung ist ein Fehler aufgetreten.\n" +
                    "Stellen Sie sicher, dass die Kamera angeschlossen ist!");
                return;
            }

            // Zug berechnen
            int state;
            string y, player;
            string nextMoveX = processNextMove(lastJSONStatus, newJSONStatus, out state, out y, out player);
            if (Convert.ToInt32(nextMoveX) < 0)
            {
                // Fehler-Auswertung und Second-Try
                switch (state)
                {
                    case 0:
                        MessageBox.Show("UNENTSCHIEDEN!");
                        break;
                    case 1:
                    case 2:
                        MessageBox.Show("Spieler " + state + " gewinnt!");
                        break;
                    case -2:
                        if (!cheat)
                        {
                            cheat = true;
                            secondChance();
                            return;
                        }
                        MessageBox.Show("CHEAT!");
                        break;
                    case -5:
                        mainTimer.Enabled = true;
                        return;
                    default:
                        MessageBox.Show("Es ist ein Fehler aufgetreten!");
                        break;
                }
                return;
            }

            if (cheat)
                cheat = false;

            // Zug durchführen
            if (!playMove(nextMoveX)) return;

            // Speichern, dass Zug bereits bekannt ist.
            safeLastJSON(newJSONStatus, nextMoveX, y, player);

            mainTimer.Enabled = true;
        }

        private void safeLastJSON(string newJSONStatus, string nextMoveX, string y, string player)
        {
            int[,] field = InputHandling.Get2DArrayFromJSON(newJSONStatus);
            field[Convert.ToInt32(nextMoveX), Convert.ToInt32(y)] = Convert.ToInt32(player);
            lastJSONStatus = InputHandling.GetJsonFrom2DArray(field);
        }

        private string imageRecognition()
        {
            string[] cameraList = getCameraList();
            int camera = cameraList.ToList().IndexOf("HD Camera 720px");
            if (camera == -1)
                return "-1"; // Kamera nicht in der Liste

            string newJSONStatus = processVision(camera);
            if (newJSONStatus == "-1")
                return "-1";
            int[,] field = InputHandling.Get2DArrayFromJSON(newJSONStatus);
            fieldView.SetField(field);
            return newJSONStatus;
        }

        private string[] getCameraList()
        {
            console.StartInfo.Arguments = "vision devices";
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output.Split(new[] {"; "}, StringSplitOptions.None);
        }

        private bool playMove(string nextMoveX)
        {
            string robotics = processRobotics(nextMoveX, ip);
            if (robotics != "1")
            {
                MessageBox.Show("Robotics Error!");
                return false;
            }
            return true;
        }


        private string processConsoleTest()
        {
            console.StartInfo.Arguments = "";
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        private string processVision(int camera)
        {
            console.StartInfo.Arguments = "vision";
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        private string processNextMove(string oldField, string newField, out int state, out string y, out string player)
        {
            state = -4;
            y = "-1";
            player = "-1";

            console.StartInfo.Arguments = oldField + " " + newField;
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();

            string[] outputArray = output.Split(new[] {" "}, StringSplitOptions.None);

            try
            {
                state = Convert.ToInt32(outputArray[3]);
                if (state < -1)
                    return outputArray[3];
            }
            catch (Exception)
            {
                return "-4";
            }

            y = outputArray[1];
            player = outputArray[2] == "1" ? "2" : "1";
            return outputArray[0];
        }

        private string processRobotics(string x, string ip="")
        {
            console.StartInfo.Arguments = "robot " + x + ((ip == "") ? "" : " " + ip);
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            fieldView.Show();
        }

        private void secondChance()
        {
            secondChanceTimer.Interval = 2000;
            secondChanceTimer.Start();
        }

        private void secondChanceTimer_Tick(object sender, EventArgs e)
        {
            secondChanceTimer.Stop();
            mainTimer_Tick(sender ,e);
        }

        
    }
}
