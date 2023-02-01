using BombPalne;
using BombPlane.Server;
using WinFormsApp2;


namespace BombPlane
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
          
            ApplicationConfiguration.Initialize();
            Server.Server.Open();
            Application.Run(new Form1());
        }
    }
}