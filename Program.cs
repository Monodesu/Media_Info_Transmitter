namespace Media_Info_To_VRChat_Discord
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            bool isRunningWithSystem = args.Contains("runningwithsystem");

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(isRunningWithSystem));
        }
    }
}