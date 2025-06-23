
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
        public List<string> FullScanCandidates { get; set; } = new();
        public string ProjectType { get; set; } = "unknown";
    }
    public static class FolderScanner
    {
        public static FolderSummary ScanFolder(string rootPath, int maxDepth = 5)
        {
            var summary = new FolderSummary() { RootPath = rootPath };
            summary.ProjectType = DetectProjectType(rootPath);
            var includedExt = summary.ProjectType switch
            {
                "dotnet" => new[] { ".cs", ".xaml", ".csproj", ".json" },
                "node"   => new[] { ".js", ".ts", ".json", ".jsx", ".tsx" },
                "python" => new[] { ".py", ".toml", ".txt" },
                "cpp"    => new[] { ".cpp", ".h", ".hpp", ".c", ".cc" },
                "unreal" => new[] { ".cpp", ".h", ".ini", ".uproject" },
                _        => Array.Empty<string>()
            };
            
            Logger.LogWorkflow($"Scanning folder: {summary.RootPath}\n" +
                               $"Detected project type: {summary.ProjectType}");
            
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
                        if(!IsFileIncluded(file)) continue;
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

                        bool isRelevant = summary.ProjectType switch
                        {
                            "dotnet" => ext == ".cs" || ext == ".xaml",
                            "node" => ext == ".js" || ext == ".ts",
                            "python" => ext == ".py",
                            "cpp" => ext == ".cpp" || ext == ".h",
                            "unreal" => ext == ".cpp" || ext == ".h" || ext == ".ini",
                            _ => false
                        };
                        if (isRelevant)
                        {
                            summary.FullScanCandidates.Add(fileInfo.FullName);
                        }
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
                        if (!IsFileIncluded(dirName)) continue;
                        
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

        public static async Task<string> FormatSummary(FolderSummary summary)
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

            Logger.LogContextMenu($"FullScanCandidates: {summary.FullScanCandidates.Count}\n");
            if (summary.FullScanCandidates.Count > 0)
            {
                lines.Add("");
                lines.Add($"🧠 AI Full-Scan Candidates ({summary.FullScanCandidates.Count}):");

                foreach (var file in summary.FullScanCandidates.Take(10))
                {
                    lines.Add($"-{Path.GetFileName(file)}");
                    string fileContent = File.ReadAllText(file);

                    const int maxTokens = 24_000;
                    const int maxChars = maxTokens * 4; // ~96,000 characters
                    int estimatedTokens = GPTProcessor.EstimateTokens(fileContent);

            
                    Logger.LogContextMenu($"[{DateTime.Now}] Analyzing file: {Path.GetFileName(file)}\n" +
                                          $"- Char length: {fileContent.Length}\n" +
                                          $"- Estimated tokens: {estimatedTokens} / {maxTokens}\n");

                    if (estimatedTokens > maxTokens)
                    {
                        try
                        {
                            Logger.LogContextMenu($"[{DateTime.Now}] File is too large. Splitting into chunks.");


                            int chunkCharSize = maxChars / 2; // ~48,000 chars per chunk
                            List<string>
                                chunks = GPTProcessor.ChunkString(fileContent,
                                    chunkCharSize); // roughly 12,000 tokens per chunk

                            string finalResponse = await GPTProcessor.StartMultiChunkCall(chunks, file);

                        }
                        catch (Exception ex)
                        {
                            Logger.LogContextMenu(
                                $"-----ChunkString() failed at {DateTime.Now} -- {ex.Message}\n{ex.StackTrace}-----");
                        }
                    }
                    else
                    {
                        string prompt = $"Please analyze and summarize the contents of the following file: \n\"{file}\" \n\n{fileContent} \n\n{fileContent.Length}";
                        string response = await GPTProcessor.CallOpenAiAsync(prompt);
                    
                    }
                    
                }

                if (summary.FullScanCandidates.Count > 10)
                {
                    lines.Add($"...and {summary.FullScanCandidates.Count - 10} more files.");
                }
            }
            
            return string.Join(Environment.NewLine, lines);
        }

        private static string DetectProjectType(string startPath)
        {
            try
            {
                string? current = startPath;
                int maxLevelsUp = 5;

                while (!string.IsNullOrEmpty(current) && maxLevelsUp-- > 0)
                {
                    var files = Directory.GetFiles(current, "*.*", SearchOption.TopDirectoryOnly);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file).ToLowerInvariant();

                        if (fileName.EndsWith(".csproj")) return "dotnet";
                        if (fileName == "package.json") return "node";
                        if (fileName == "pyproject.toml" || fileName == "setup.py") return "python";
                        if (fileName.EndsWith(".uproject")) return "unreal";
                        if (fileName == "CMakeLists.txt") return "cpp";
                    }

                    current = Directory.GetParent(current)?.FullName;
                }
                
                
            }
            catch (Exception ex)
            {
                Logger.LogWorkflow(ex, $"=====Folder Scanner Failed=====\n====={startPath}=====");
            }

            return "unknown";
        }

        static readonly string[] IgnoredExtensions = { ".dll", ".exe", ".pdb", ".cache", ".db" };
        static readonly string[] IgnoredFileEndings =
        {
            ".designer.cs", ".g.cs", ".g.i.cs", ".ref.cs",
            ".assemblyinfo.cs", ".csproj.user"
        };

        static readonly string[] IgnoredDirectories =
        {
            "bin", "obj", ".vs", ".git", ".idea", "node_modules",
            "packages", "dist", "build", "__pycache__"
        };

        static bool IsFileIncluded(string filePath)
        {
            var fileName = Path.GetFileName(filePath).ToLowerInvariant();
            var ext = Path.GetExtension(fileName);
            var dirParts = filePath.ToLowerInvariant().Split(Path.DirectorySeparatorChar);

            if (IgnoredExtensions.Contains(ext)) return false;
            if (IgnoredFileEndings.Any(ending => fileName.EndsWith(ending))) return false;
            if (dirParts.Any(dir => IgnoredDirectories.Contains(dir))) return false;

            return true;
        }
    }
}