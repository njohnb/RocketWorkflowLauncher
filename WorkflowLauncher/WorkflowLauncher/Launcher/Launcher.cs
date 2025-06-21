using System.Diagnostics;
using System;

public static class Launcher
{
    public static void LaunchProfileByName(string profileName)
    {
        var profile = WorkflowManager.LoadProfile(profileName);
        foreach (var item in profile.Items)
            Launch(item);
    }
    public static void Launch(WorkflowItem item)
    {
        if (!item.Enabled) return;

        var processInfo = new ProcessStartInfo
        {
            FileName = item.PathOrURL,
            UseShellExecute = true
        };
        if (item.Type == LaunchType.Program && item.PathOrURL.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            processInfo.Arguments = item.Arguments;
        }
        
        Process.Start(processInfo);
    }
}