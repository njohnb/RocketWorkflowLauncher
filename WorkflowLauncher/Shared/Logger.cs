using System;
using System.IO;
using System.Text;

namespace WorkflowLauncher.Shared
{
    public static class Logger
    {
        private static readonly string LogDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "WorkflowLauncher");

        private static readonly string WorkflowLog = Path.Combine(LogDirectory, "launcher.log");
        private static readonly string ContextMenuLog = Path.Combine(LogDirectory, "contextmenu.log");

        static Logger()
        {
            Directory.CreateDirectory(LogDirectory); // Safe to call repeatedly

            // Create empty files if they don't exist
            if (!File.Exists(WorkflowLog))
                File.WriteAllText(WorkflowLog, "== Workflow Launcher Log ==\n");
            if (!File.Exists(ContextMenuLog))
                File.WriteAllText(ContextMenuLog, "== Context Menu Log ==\n");
        }

        public static void LogWorkflow(string message)
        {
            File.AppendAllText(WorkflowLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n\n");
        }

        public static void LogWorkflow(Exception ex, string context = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== EXCEPTION OCCURRED ===");
            if (!string.IsNullOrWhiteSpace(context))
                sb.AppendLine($"Context: {context}");
    
            sb.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"Source: {ex.Source}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");
    
            if (ex.InnerException != null)
            {
                sb.AppendLine("---- Inner Exception ----");
                sb.AppendLine($"Message: {ex.InnerException.Message}");
                sb.AppendLine($"Source: {ex.InnerException.Source}");
                sb.AppendLine($"StackTrace: {ex.InnerException.StackTrace}");
            }

            sb.AppendLine("=========================");
            File.AppendAllText(WorkflowLog, sb.ToString());
        }
        public static void LogContextMenu(string message)
        {
            File.AppendAllText(ContextMenuLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n\n");
        }

        public static void LogContextMenu(Exception ex, string context = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== EXCEPTION OCCURRED ===");
            if (!string.IsNullOrWhiteSpace(context))
                sb.AppendLine($"Context: {context}");
    
            sb.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"Source: {ex.Source}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");
    
            if (ex.InnerException != null)
            {
                sb.AppendLine("---- Inner Exception ----");
                sb.AppendLine($"Message: {ex.InnerException.Message}");
                sb.AppendLine($"Source: {ex.InnerException.Source}");
                sb.AppendLine($"StackTrace: {ex.InnerException.StackTrace}");
            }
            sb.AppendLine("=========================");
            File.AppendAllText(ContextMenuLog, sb.ToString());
        }
    }
}