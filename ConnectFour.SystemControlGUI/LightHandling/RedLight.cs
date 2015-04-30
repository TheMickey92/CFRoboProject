namespace ConnectFour.SystemControlGUI.LightHandling
{
    public class RedLight : LightHandler

    {
        public RedLight(string pathToConsole) : base(pathToConsole)
        {
        }

        protected override void setParameter()
        {
            console.StartInfo.Arguments = "robot led red";
        }
    }
}
