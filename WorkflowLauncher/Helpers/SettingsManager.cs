using Newtonsoft.Json;

namespace WorkflowLauncher;

public static class SettingsManager
{
    private static readonly string SettingsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WorkflowLauncher");

    private static readonly string SettingsPath = Path.Combine(SettingsFolder, "settings.json");
    
    public class AppSettings
    {
        public string DefaultProfile { get; set; }
    }

    private static AppSettings currentSettings = Load();
    
    public static string GetDefaultProfile() => currentSettings.DefaultProfile;

    public static void SetDefaultProfile(string profileName)
    {
        currentSettings.DefaultProfile = profileName;
        Save();
    }

    private static AppSettings Load()
    {
        if(!Directory.Exists(SettingsFolder))
            Directory.CreateDirectory(SettingsFolder);
        if(!File.Exists(SettingsPath))
            return new AppSettings();
        
        string json = File.ReadAllText(SettingsPath);
        return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
    }

    private static void Save()
    {
        if(!Directory.Exists(SettingsFolder))
            Directory.CreateDirectory(SettingsFolder);
        string json = JsonConvert.SerializeObject(currentSettings, Formatting.Indented);
        File.WriteAllText(SettingsPath, json);
    }
    
    
    
}