
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
        public static FolderSummary ScanFolder(string rootPath, int maxDepth = 5, List<string>? excludedDirs = null)
        {
            var summary = new FolderSummary() { RootPath = rootPath };
            excludedDirs ??= new List<string> { "bin", "obj", ".git", ".vs", "node_modules", ".idea" };

            void Recurse(string path, int depth)
            {
                if (depth > maxDepth) return;
                
                IEnumerable<string> files = Enumerable.Empty<string>();
                try
                {
                    files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    Logger.LogWorkflow(ex, $"=====Folder Scanner Failed=====\n====={path}=====");
                }

                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        string ext = fileInfo.Extension.ToLowerInvariant();
                        
                        summary.TotalFiles++;
                        summary.TotalSizeBytes += fileInfo.Length;
                        
                        if(!summary.FileTypeCounts.ContainsKey(ext))
                            summary.FileTypeCounts[ext] = 0;
                        summary.FileTypeCounts[ext]++;
                        
                        if(!summary.FileTypeSizes.ContainsKey(ext))
                            summary.FileTypeSizes[ext] = 0;
                        summary.FileTypeSizes[ext] += fileInfo.Length;
                        
                        summary.LargestFiles.Add((fileInfo.FullName, fileInfo.Length));
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWorkflow(ex, $"=====Folder Scanner Failed=====\n====={file}=====");
                    }
                }
                IEnumerable<string> subdirs = Enumerable.Empty<string>();
                try
                {
                    subdirs = Directory.EnumerateDirectories(path);
                }
                catch (Exception ex)
                {
                    Logger.LogWorkflow(ex, $"=====Folder Scanner Failed=====\n====={path}=====");
                }

                foreach (var dir in subdirs)
                {
                    try
                    {
                        string dirName = Path.GetFileName(dir);
                        if (excludedDirs.Any(ex => string.Equals(ex, dirName, StringComparison.OrdinalIgnoreCase)))
                            continue;
                        
                        Recurse(dir, depth + 1);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWorkflow(ex, $"=====Folder Scanner Failed=====\n====={dir}=====");
                    }
                }
            }
            
            Recurse(rootPath, 0);

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