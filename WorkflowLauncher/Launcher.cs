using System.Diagnostics;
using System;

public static class Launcher
{
    public static void Launch(WorkflowItem item)
    {
        if (!item.Enabled) return;

        switch (item.Type)
        {
            case LaunchType.Program:
                // launch program
                Process.Start(new ProcessStartInfo
                {
                    FileName = item.PathOrURL,
                    Arguments = item.Arguments,
                    UseShellExecute = false,
                });
                break;
            case LaunchType.Folder:
                Process.Start(new ProcessStartInfo
                {
                    FileName = item.PathOrURL,
                    UseShellExecute = true,
                });
                break;
            case LaunchType.Website:
                Process.Start(new ProcessStartInfo
                {
                    FileName = item.PathOrURL,
                    UseShellExecute = true,
                });
                break;
            case LaunchType.Custom:
                // extend this later
                break;
            default:
                throw new InvalidOperationException($"Unsupported launch type: {item.Type}");
        }
    }
}