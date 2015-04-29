using System;
using System.Diagnostics;
using System.Threading;

namespace ConnectFour.SystemControlGUI.LightHandling
{
    public abstract class LightHandler
    {
        private Thread thread;
        protected Process console;
        private string pathToConsole;

        protected LightHandler(string pathToConsole)
        {
            this.pathToConsole = pathToConsole;
            initializeProcess();
        }
        private void initializeProcess()
        {
            console = new Process();
            console.StartInfo.FileName = pathToConsole;
            console.StartInfo.UseShellExecute = false;
            console.StartInfo.RedirectStandardOutput = true;
            console.StartInfo.CreateNoWindow = true;
            setParameter();
        }

        protected abstract void setParameter();

        public void On()
        {
            thread = new Thread(workConsole);
            thread.Start();
        }

        public void Off()
        {
            try
            {
                thread.Abort();
                console.Close();
            }
            catch (Exception)
            {
                
            }
        }

        private void workConsole()
        {
            while (true)
            {
                try
                {
                    console.Start();
                    console.WaitForExit();
                    console.Close();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}
