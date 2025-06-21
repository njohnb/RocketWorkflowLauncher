using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WorkflowContextMenu
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class AIContextMenu : SharpContextMenu
    {
        protected override bool CanShowMenu() => SelectedItemPaths != null;
        protected override ContextMenuStrip CreateMenu()
        {
            ///////////////////// DEBUGGING /////////////////////
            string logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RocketWorkflowLogs",
                "contextmenu.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            File.AppendAllText(logPath, $"CreateMenu called at {DateTime.Now}\n");
            /////////////////////////////////////////////////////

            
            var menu = new ContextMenuStrip();
            var menuItem = new ToolStripMenuItem()
            {
                Text = "Send to AI Assistant"
            };
            menuItem.Click += (s, e) => HandleClick(SelectedItemPaths);
            menu.Items.Add(menuItem);
            return menu;
        }

        private void HandleClick(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                
                ///////////////////// DEBUGGING /////////////////////
                #if DEBUG
                string exePath = @"C:\Projects\WorkflowLauncher\WorkflowLauncher\WorkflowLauncher\WorkflowLauncher\bin\Debug\net9.0-windows\WorkflowLauncher.exe"
                #else
                string exePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "WorkflowLauncher",
                    "WorkflowLauncher.exe");
                #endif
                
                ///////////////////// DEBUGGING /////////////////////
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RocketWorkflowLogs",
                    "contextmenu.log");
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                File.AppendAllText(logPath, $"HandleClick called at {DateTime.Now}\n" +
                                            $"Sending {path} to {exePath}\n");
                /////////////////////////////////////////////////////
                /// 
                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"--analyze \"{path}\"",
                    UseShellExecute = true
                });
            }
        }
    }
}