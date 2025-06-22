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
            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WorkflowLauncher", "Log_ContextMenu.txt");
            File.AppendAllText(logPath,
                "--------------CREATEMENU ENTERED!");
            
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
            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WorkflowLauncher", "Log_ContextMenu.txt");
            File.AppendAllText(logPath,
                "--------------HandleClick ENTERED!");
            foreach (var path in paths)
            {
                string exePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "WorkflowLauncher",
                    "WorkflowLauncher.exe");
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