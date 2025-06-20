public class WorkflowProfile
{
    public string ProfileName { get; set; }
    public List<WorkflowItem> Items { get; set; } = new();
}