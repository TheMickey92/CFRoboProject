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
            NextMove nextMove = processNextMove(lastJSONStatus, newJSONStatus);
            if (Convert.ToInt32(nextMove) < 0)
            {
                // Fehler-Auswertung und Second-Try
                switch (nextMove.State)
                {
                    case 0:
                        MessageBox.Show("UNENTSCHIEDEN!");
                        break;
                    case 1:
                    case 2:
                        MessageBox.Show("Spieler " + nextMove.State + " gewinnt!");
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
            if (!playMove(nextMove.X)) return;

            // Speichern, dass Zug bereits bekannt ist.
            safeLastJSON(newJSONStatus, nextMove.X, nextMove.Y, nextMove.Player);

            mainTimer.Enabled = true;
        }

        private void safeLastJSON(string newJSONStatus, int x, int y, int player)
        {
            int[,] field = InputHandling.Get2DArrayFromJSON(newJSONStatus);
            field[x, y] = player;
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

        private bool playMove(int nextMoveX)
        {
            string robotics = processRobotics(nextMoveX, ip);
            if (robotics != "1")
            {
                MessageBox.Show("Robotics Error!");
                return false;
            }
            return true;
        }

        private string processVision(int camera)
        {
            console.StartInfo.Arguments = "vision";
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();
            return output;
        }

        private NextMove processNextMove(string oldField, string newField)
        {
            int state = -4;
            int y = -1, x = -1;
            int player = -1;

            console.StartInfo.Arguments = oldField + " " + newField;
            console.Start();
            string output = console.StandardOutput.ReadToEnd();
            console.WaitForExit();

            string[] outputArray = output.Split(new[] {" "}, StringSplitOptions.None);

            try
            {
                state = Convert.ToInt32(outputArray[3]);
                if (state < -1)
                    return new NextMove(-1, -1, -1 , state);
            }
            catch (Exception)
            {
                return new NextMove(-1, -1, -1, -4);
            }

            y = Convert.ToInt32(outputArray[1]);
            player = Convert.ToInt32(outputArray[2] == "1" ? "2" : "1");
            x= Convert.ToInt32(outputArray[0]);
            return new NextMove(x, y, player, state);
        }

        private string processRobotics(int x, string ip="")
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
