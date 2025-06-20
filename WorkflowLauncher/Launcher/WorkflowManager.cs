using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class WorkflowManager
{
    public static string ProfilesDir
    {
        get
        {
#if DEBUG
            // This version goes up to your project root during development
            string devPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\Profiles"));
            if (!Directory.Exists(devPath))
                Directory.CreateDirectory(devPath);
            return devPath;
#else
        // Installed location (next to EXE)
        string appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RocketWorkflowLauncher",
            "Profiles");
            if(!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            return appDataPath;
#endif
        }
    }
    public static List<string> GetAvailableProfiles()
    {
        if(!Directory.Exists(ProfilesDir))
            Directory.CreateDirectory(ProfilesDir);

        var files = Directory.GetFiles(ProfilesDir, "*.json");
        var profileNames = new List<string>();
        foreach (var file in files)
            profileNames.Add(Path.GetFileNameWithoutExtension(file));
        return profileNames;
    }

    public static WorkflowProfile LoadProfile(string profileName)
    {
        var path = Path.Combine(ProfilesDir, profileName + ".json");
        if (!File.Exists(path))
            return new WorkflowProfile { ProfileName = profileName };
        
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<WorkflowProfile>(json);
    }

    public static void SaveProfile(WorkflowProfile profile)
    {
        var path = Path.Combine(ProfilesDir, profile.ProfileName + ".json");
        var json = JsonConvert.SerializeObject(profile, Formatting.Indented);
        File.WriteAllText(path, json);
    }
}