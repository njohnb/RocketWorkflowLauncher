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
            var menu = new ContextMenuStrip();
            var SendToAiAssistantButton = new ToolStripMenuItem()
            {
                Text = "Send to AI Assistant"
            };
            SendToAiAssistantButton.Click += (s, e) => HandleClick(SelectedItemPaths);
            
            menu.Items.Add(new ToolStripSeparator());
            
            menu.Items.Add(SendToAiAssistantButton);
            
            menu.Items.Add(new ToolStripSeparator());
            
            
            return menu;
        }

        private void HandleClick(IEnumerable<string> paths)
        {
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