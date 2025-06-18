using Microsoft.Win32;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WorkflowLauncher
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

            bool createdNew;
            using (Mutex mutex = new Mutex(true, "WorkflowLauncher_Mutex", out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("Workflow Launcher is already running.", "Workflow Launcher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}