using Microsoft.Win32;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace WorkflowLauncher
{
    internal static class Program
    {
        private static Mutex _mutex;
        public static MainForm mainForm;
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // handle analyze logic separate so parallel AI windows can be open
            if (args.Length >= 2 && args[0] == "--analyze")
            {
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RocketWorkflowLogs",
                    "contextmenu.log");
                File.AppendAllText(logPath, $"analyze attempted!!! {DateTime.Now}\n");

                ApplicationConfiguration.Initialize();
                Application.Run(new OpenAIForm(args[1]));
                return;
            }
            
            bool createdNew = true;
            _mutex = new Mutex(true, "WorkflowLauncher_Mutex", out createdNew);
            if (createdNew)
            {
                Thread pipeThread = new Thread(ListenForPipeMessages)
                {
                    IsBackground = true
                };
                pipeThread.Start();
                
                ApplicationConfiguration.Initialize();
                
                // check if launching profile from shortcut
                if (args.Length >= 2 && args[0] == "--profile")
                {
                    Launcher.LaunchProfileByName(args[1]);
                }
                else
                {
                    WorkflowProfile startupProfile = WorkflowManager.GetStartupProfile();
                    if (startupProfile != null)
                    {
                        // launch all items on app startup
                        Launcher.LaunchProfileByName(startupProfile.ProfileName);
                    }
                }
                
                mainForm = new MainForm();
                Application.Run(mainForm);
            }
            else
            {
                try
                {
                    // second instance - send a message to show the window
                    using (NamedPipeClientStream client =
                           new NamedPipeClientStream(".", "WorkflowLauncherPipe", PipeDirection.Out))
                    {
                        client.Connect(3000); // wait a second
                        using (StreamWriter writer = new StreamWriter(client))
                        {
                            if (args.Length >= 2 && args[0] == "--profile")
                            {
                                string profileName = args[1];
                                Launcher.LaunchProfileByName(profileName);
                            }
                            else
                            {
                                writer.WriteLine("SHOW");
                                writer.Flush();
                            }
                            
                            
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        static void ListenForPipeMessages()
        {
                while (true)
                {
                    try
                    {
                        using (NamedPipeServerStream pipeServer =
                               new NamedPipeServerStream("WorkflowLauncherPipe", PipeDirection.In))
                        {
                            pipeServer.WaitForConnection();
                            
                            using (StreamReader reader = new StreamReader(pipeServer))
                            {
                                string message = reader.ReadLine();
                                if (message == "SHOW" && mainForm != null)
                                {
                                    mainForm.Invoke(() =>
                                    {

                                        mainForm.Show();
                                        mainForm.BringToFront();
                                        mainForm.Activate();
                                        if (mainForm.WindowState == FormWindowState.Minimized)
                                            mainForm.WindowState = FormWindowState.Normal;
                                    });
                                }
                            }
                        }
                    }
                    catch
                    {
                        // log or ignore
                    }
                }
        }
    }
}