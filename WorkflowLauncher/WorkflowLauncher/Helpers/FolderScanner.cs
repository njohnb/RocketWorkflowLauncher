
using WorkflowLauncher.Shared;

namespace WorkflowLauncher
{
    public class FolderSummary
    {
        public string RootPath { get; set; }
        public int TotalFiles { get; set; }
        public long TotalSizeBytes { get; set; }
        public Dictionary<string, int> FileTypeCounts { get; set; } = new();
        public Dictionary<string, long> FileTypeSizes { get; set; } = new();
        public List<(string Name, long Size)> LargestFiles { get; set; } = new();
    }
    public static class FolderScanner
    {
        public static FolderSummary ScanFolder(string rootPath)
        {
            var summary = new FolderSummary() { RootPath = rootPath };
            var allFiles = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    string ext = fileInfo.Extension.ToLowerInvariant();

                    summary.TotalFiles++;
                    summary.TotalSizeBytes += fileInfo.Length;

                    if (!summary.FileTypeCounts.ContainsKey(ext))
                        summary.FileTypeCounts[ext] = 0;
                    summary.FileTypeCounts[ext]++;

                    if (!summary.FileTypeSizes.ContainsKey(ext))
                        summary.FileTypeSizes[ext] = 0;
                    summary.FileTypeSizes[ext] += fileInfo.Length;

                    summary.LargestFiles.Add((fileInfo.FullName, fileInfo.Length));

                }
                catch (Exception ex)
                {
                    Logger.LogWorkflow(ex, "=====Folder Scanner Failed=====");
                }
            }

            summary.LargestFiles = summary.LargestFiles
                .OrderByDescending(f => f.Size)
                .Take(5)
                .ToList();

            return summary;
        }

        public static string FormatSummary(FolderSummary summary)
        {
            var lines = new List<string>
            {
                $"📁 Folder Summary: {summary.RootPath}",
                $"Total Files: {summary.TotalFiles}",
                $"Total Size: {summary.TotalSizeBytes / (1024.0 * 1024.0):F2} MB",
                "",
                "Most Common File Types:",
            };

            foreach (var kvp in summary.FileTypeCounts.OrderByDescending(k => k.Value).Take(10))
            {
                lines.Add($"- {kvp.Key}: {kvp.Value} files");
            }

            lines.Add("");
            lines.Add("Top 5 Largest Files");
            foreach (var file in summary.LargestFiles)
            {
                lines.Add($"- {Path.GetFileName(file.Name)} ({file.Size / 1024.0 / 1024.0:F2} MB)");
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}