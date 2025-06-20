public enum LaunchType
{
    Program,
    Website,
    Folder,
    Custom
}

public class WorkflowItem
{
    public string Name { get; set; }
    public string PathOrURL { get; set; }
    public string Arguments { get; set; }
    public LaunchType Type { get; set; }
    public bool Enabled { get; set; }
}
