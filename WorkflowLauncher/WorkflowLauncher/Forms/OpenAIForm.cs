using System.Text;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WorkflowLauncher;

public partial class OpenAIForm : Form
{
    private const string AzureFunctionBaseUrl = "https://querygptkey.azurewebsites.net/api/QueryGPT";
    private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    public OpenAIForm()
    {
        InitializeComponent();
    }
    private string targetPath;
    public OpenAIForm(string targetPath = null)
    {
        InitializeComponent();
        this.targetPath = targetPath;
        Shown += OpenAIForm_Shown;
    }

    private async void OpenAIForm_Shown(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(targetPath))
            return;
        try
        {
            buttonSendPrompt.Enabled = false;
            string fileContent = File.ReadAllText(targetPath);

            const int maxTokens = 24_000;
            const int maxChars = maxTokens * 4; // ~96,000 characters
            int estimatedTokens = EstimateTokens(fileContent);

            ///////// DEBUGGING /////////
            string logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RocketWorkflowLogs",
                "contextmenu.log");
            File.AppendAllText(logPath,
                $"[{DateTime.Now}] Analyzing file: {targetPath}\n" +
                $"- Char length: {fileContent.Length}\n" +
                $"- Estimated tokens: {estimatedTokens} / {maxTokens}\n");
            /////////////////////////////

            if (estimatedTokens > maxTokens)
            {
                try
                {
                    File.AppendAllText(logPath,
                    $"----- Starting multi-chunk analysis, calling StartMultiChunkCall()");
                    int chunkCharSize = maxChars / 2; // ~48,000 chars per chunk
                    List<string> chunks = ChunkString(fileContent, chunkCharSize); // roughly 12,000 tokens per chunk
                    
                    File.AppendAllText(logPath, $"--> About to call StartMultiChunkCall() at {DateTime.Now}\n");
                    await StartMultiChunkCall(chunks, targetPath);
                    File.AppendAllText(logPath, $"--> Successfully returned from StartMultiChunkCall() at {DateTime.Now}\n");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logPath,
                        $"-----ChunkString() failed at {DateTime.Now} -- {ex.Message}\n{ex.StackTrace}");
                }
            }
            else
            {
                string prompt =
                    $"Please analyze and summarize the contents of the following file: \n\"{targetPath}\" \n\n{fileContent} \n\n{fileContent.Length}";
                textBoxPrompt.Text = prompt;
                textBoxResponse.Text = "Thinking...";
                await StartAsyncCall(prompt);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error reading file at: {targetPath}\nException: {ex.Message}");
        }
        finally
        {
            buttonSendPrompt.Enabled = true;
        }
    }
    
    private int EstimateTokens(string text) => (int)(text.Length / 4.0);
    
    private List<string> ChunkString(string content, int chunkSize)
    {
        string logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RocketWorkflowLogs",
            "contextmenu.log");
        File.AppendAllText(logPath, $"ChunkString() called, total content length: {content.Length}, chunk size: {chunkSize}\n");
        
        List<string> chunks = new();
        int start = 0;
        while (start < content.Length)
        {
            int length = Math.Min(chunkSize, content.Length - start);

            int searchStart = start + length - 1;
            if(searchStart >= content.Length)
                searchStart = content.Length - 1;

            int lastNewLine = content.LastIndexOf('\n', searchStart, length);

            if (lastNewLine <= start || lastNewLine == -1)
            {
                lastNewLine = start + length;
                if(lastNewLine > content.Length)
                    lastNewLine = content.Length;
            }
            int chunkLength = lastNewLine - start;
            chunks.Add(content.Substring(start, chunkLength).Trim());

            start = lastNewLine;
        }
        
        File.AppendAllText(logPath, $"Chunking complete. Number of chunks: {chunks.Count}\n");
        return chunks;
    }

    private async Task StartMultiChunkCall(List<string> chunks, string targetPath)
    {
        ///////// DEBUGGING /////////
        string logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RocketWorkflowLogs",
            "contextmenu.log");

        try
        {

            File.AppendAllText(logPath,
                $"--------------STARTMULTICHUNKCALL ENTERED!");
            /////////////////////////////

            buttonSendPrompt.Enabled = false;
            textBoxResponse.Text = "Analyzing large file in parts...";

            StringBuilder combinedSummary = new();
            for (int i = 0; i < chunks.Count; i++)
            {
                string chunkPrompt =
                    $"This is chunk {i + 1} of {chunks.Count} from file \"{targetPath}\":\n\n{chunks[i]}";
                string response = await CallOpenAIAsync(chunkPrompt, true);
                combinedSummary.AppendLine($"### Summary of chunk {i + 1}:\n{response}");
                File.AppendAllText(logPath,
                    $"------------Chunk #: {i + 1} of {chunks.Count}\n");
            }

            string finalPrompt =
                $"Please review the following chunk summaries of the file \"{Path.GetFileName(targetPath)}\" and provide a full consolidated analysis:\n\n{combinedSummary}";
            string finalResponse = await CallOpenAIAsync(finalPrompt, true);
            textBoxResponse.Text = finalResponse;
        }
        catch (Exception ex)
        {
            File.AppendAllText(logPath,
                $"Failure in StartMultiChunkCall at {DateTime.Now}: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            buttonSendPrompt.Enabled = true;
        }

    }
    private async Task StartAsyncCall(string analysisPrompt)
    {
        string response = await CallOpenAIAsync(analysisPrompt, true);
        textBoxResponse.Text = response;
    }


    private async Task<string> CallOpenAIAsync(string userInput, bool bAnalyze = false)
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
                return $"Request to Azure Function failed: {ex.Message}";
            }
        }
    }

    private async void buttonSendPrompt_Click_1(object sender, EventArgs e)
    {
        buttonSendPrompt.Enabled = false;
        textBoxResponse.Text = "Thinking...";
        string input = textBoxPrompt.Text.Trim();
        if (!string.IsNullOrEmpty(input))
        {
            string response = await CallOpenAIAsync(input, false);
            textBoxResponse.Text = response;
        }

        buttonSendPrompt.Enabled = true;
    }

    private void buttonDownloadResponse_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBoxResponse.Text) || string.Equals(textBoxResponse.Text, "Thinking..."))
        {
            MessageBox.Show("No response to download.","Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.Title = "Save OpenAI Response";
            saveFileDialog.FileName = "OpenAIResponse.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, textBoxResponse.Text);
                    MessageBox.Show("Response saved successfully.", "Saved", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Failed to save response: {exception.Message}", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}