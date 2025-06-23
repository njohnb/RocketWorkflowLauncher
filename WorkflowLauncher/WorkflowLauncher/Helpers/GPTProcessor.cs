using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowLauncher.Shared;

namespace WorkflowLauncher;

public static class GPTProcessor
{

    private const string AzureFunctionBaseUrl = "https://querygptkey.azurewebsites.net/api/QueryGPT";
    public static int EstimateTokens(string text) => (int)(text.Length / 4.0);
    public static List<string> ChunkString(string content, int chunkSize)
    {
        Logger.LogContextMenu(
            $"ChunkString() called, total content length: {content.Length}, chunk size: {chunkSize}\n");

        List<string> chunks = new();
        int start = 0;
        while (start < content.Length)
        {
            int length = Math.Min(chunkSize, content.Length - start);

            int searchStart = start + length - 1;
            if (searchStart >= content.Length)
                searchStart = content.Length - 1;

            int lastNewLine = content.LastIndexOf('\n', searchStart, length);

            if (lastNewLine <= start || lastNewLine == -1)
            {
                lastNewLine = start + length;
                if (lastNewLine > content.Length)
                    lastNewLine = content.Length;
            }

            int chunkLength = lastNewLine - start;
            chunks.Add(content.Substring(start, chunkLength).Trim());

            start = lastNewLine;
        }

        Logger.LogContextMenu($"ChunkString() complete. Number of chunks: {chunks.Count}\n");
        return chunks;
    }

    public static async Task<string> StartMultiChunkCall(List<string> chunks, string targetPath)
    {
        try
        {

            StringBuilder combinedSummary = new();
            for (int i = 0; i < chunks.Count; i++)
            {
                string chunkPrompt =
                    $"This is chunk {i + 1} of {chunks.Count} from file \"{targetPath}\":\n\n{chunks[i]}";
                string response = await CallOpenAiAsync(chunkPrompt, true);
                combinedSummary.AppendLine($"### Summary of chunk {i + 1}:\n{response}");
                Logger.LogContextMenu($"------------Chunk #: {i + 1} of {chunks.Count}\n");
            }

            string finalPrompt =
                $"Please review the following chunk summaries of the file \"{Path.GetFileName(targetPath)}\" and provide a full consolidated analysis:\n\n{combinedSummary}";
            return await CallOpenAiAsync(finalPrompt, true);

        }
        catch (Exception ex)
        {
            Logger.LogContextMenu(ex, "-----StartMultiChunkCall failed.-----");
        }
        return "UNKNOWN FAILURE IN GPTPROCESSOR.STARTMULTICHUNKCALL()";
    }
    public static async Task<string> CallOpenAiAsync(string userInput, bool bAnalyze = false)
    {
        var requestData = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = bAnalyze
                ? "You are a code analysis and project insight AI assistant. " +
                  "Your role is to take source code files and folders from the user and convert them into a clear, structured, and actionable overview. " +
                  "You help the user understand codebases by extracting relevant technical details, surfacing architecture, and identifying relationships between components.\n\n" +
                  "Always respond with:\n" +
                  "1. Project Summary – A concise overview of the project’s purpose and structure based on the files/folders provided.\n" +
                  "2. Key Components – A breakdown of significant classes, methods, or functions. For each, provide:\n" +
                  "   - Name and type (e.g., class, function, interface)\n" +
                  "   - Purpose\n" +
                  "   - Key parameters and return types (if available)\n" +
                  "   - Relationships (e.g., inheritance, imports, usage)\n" +
                  "3. File & Folder Overview – A bullet list summarizing each file or folder’s purpose and contents.\n" +
                  "4. Notable Dependencies or Configurations – Highlight important package references, external libraries, build configs, or environment settings (e.g., package.json, .csproj, requirements.txt).\n" +
                  "5. Potential Issues or Observations – Mention deprecated code, duplication, poor practices, or areas of architectural concern (if any).\n" +
                  "6. Developer-Facing Insights – If applicable, offer suggestions that would help a new developer quickly onboard into the project.\n\n" +
                  "Guidelines:\n" +
                  "- Use clear, technical language.\n" +
                  "- Structure output using Markdown-style headings and bullet points where helpful.\n" +
                  "- Be accurate; do not hallucinate or generate code unless explicitly asked to.\n" +
                  "- If files are missing or incomplete, note the gaps but do not make unwarranted assumptions."
                  
                : "You are a technical project architect AI. " +
                  "Your role is to take abstract or vague software, app, game, or utility ideas from the user " +
                  "and convert them into a clear, structured, and actionable project outline.\n\n" +
                  "Always respond with:\n" +
                  "1. A concise summary of the idea in your own words.\n" +
                  "2. Key features and functionality.\n" +
                  "3. Recommended tech stack (languages, frameworks, tools, engines).\n" +
                  "4. A detailed development roadmap including:\n" +
                  "   - Phases (e.g., Planning, Prototyping, Core Development, Polish, Release)\n" +
                  "   - Core modules and systems to be built\n" +
                  "   - Milestones (e.g., MVP target, Alpha, Beta, Launch)\n" +
                  "   - Suggested order of implementation based on technical dependencies\n" +
                  "5. Optional tools or libraries to speed up development.\n" +
                  "6. Any risks, limitations, or architectural considerations to keep in mind.\n\n" +
                  "Assume the user has development experience unless stated otherwise. " +
                  "Don't wait for clarification unless the idea is completely ambiguous — " +
                  "use reasonable assumptions and clearly note them. Be precise and use a practical, " +
                  "engineering-driven tone.\n\n" +
                  "Format the outline for easy export as a formatted document."
                },
                new
                {
                    role = "user",
                    content = userInput
                }
            },
            temperature = 0.7
        };
        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        using (var httpClient = new HttpClient())
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("x-api-key", "0246813579");
                
                string azureFunctionUrl = AzureFunctionBaseUrl;// + "?code=" + key;
                var response = await httpClient.PostAsync(azureFunctionUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: {response.StatusCode}\n{responseString}";
                }
                
                var json = JObject.Parse(responseString);
                var message = json["choices"]?[0]?["message"]?["content"]?.ToString()?.Trim();
                return message ?? "No response from Azure Function.";
            }
            catch (Exception ex)
            {
                Logger.LogWorkflow($"GPTProcess.CallOpenAIAsync() failed: {ex.Message}\n{ex.StackTrace}\n\n");
            }
        }
        return "UNKNOWN FAILURE IN GPTPROCESSOR.CALLOPENAIASYNC()";
    }
}