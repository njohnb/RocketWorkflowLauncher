
namespace WorkflowLauncher;

public static class ShortcutHelper
{
    public static void CreateShortcut(string shortcutPath, string targetPath, string arguments = "")
    {
        Type shellType = Type.GetTypeFromProgID("WScript.Shell");

        if (shellType == null)
        {
            throw new InvalidOperationException("WScript.Shell not found");
        }
        
        dynamic shell = Activator.CreateInstance(shellType);
        dynamic shortcut = shell.CreateShortcut(shortcutPath);
        
        shortcut.TargetPath = targetPath;
        
        string baseDir = AppContext.BaseDirectory;
        string iconPath = Path.Combine(baseDir, "Assets", "rocket.ico");
        if(File.Exists(iconPath))
            shortcut.IconLocation = iconPath;
        
        shortcut.Arguments = arguments;
        shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
        shortcut.WindowStyle = 1;
        shortcut.Description = "WorkflowLauncher";
        shortcut.Save();
        
        
        
    }
}