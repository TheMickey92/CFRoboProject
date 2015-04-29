namespace ConnectFour.SystemControlGUI.LightHandling
{
    public class GreenLight : LightHandler
    {
        public GreenLight(string pathToConsole) : base(pathToConsole)
        {
        }

        protected override void setParameter()
        {
            console.StartInfo.Arguments = "robot led green";
        }
    }
}
