using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WorkflowLauncher;

public partial class OpenAIForm : Form
{
    private static string apiKey = Program.AppSecrets.OpenAiKey;
    private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    public OpenAIForm()
    {
        InitializeComponent();
    }

    private async Task<string> CallOpenAIAsync(string userInput)
    {
        var requestData = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new
                {
                    role = "system", content =
                        "You are a technical project architect AI. " +
                        "Your role is to take abstract or vague software, app, game, or utility ideas from the user" +
                        " and convert them into a clear, structured, and actionable project outline.\n\n" +
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
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject json = JObject.Parse(responseString);
                return json["choices"]?[0]?["message"]?["content"]?.ToString()?.Trim() ?? "No response.";
                
            }
            else
            {
                return $"Error: {response.StatusCode}\n{responseString}";
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
            string response = await CallOpenAIAsync(input);
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