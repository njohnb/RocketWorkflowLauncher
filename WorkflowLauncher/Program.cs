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
            _ = AppSecrets.OpenAiKey;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

        }
        public static class AppSecrets
        {

            private static readonly IConfigurationRoot Configuration;
            static AppSecrets()
            {
                var builder = new ConfigurationBuilder()
                    .AddUserSecrets(typeof(Program).Assembly);
            
                Configuration = builder.Build();
            }
        
            public static string OpenAiKey => Configuration["OpenAI:ApiKey"];
        }
    
    

    }
}