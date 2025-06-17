using System.ComponentModel;

namespace WorkflowLauncher;

partial class OpenAIForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        textBoxResponse = new System.Windows.Forms.TextBox();
        buttonSendPrompt = new System.Windows.Forms.Button();
        buttonDownloadResponse = new System.Windows.Forms.Button();
        textBoxPrompt = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // textBoxResponse
        // 
        textBoxResponse.Location = new System.Drawing.Point(19, 276);
        textBoxResponse.Multiline = true;
        textBoxResponse.Name = "textBoxResponse";
        textBoxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBoxResponse.Size = new System.Drawing.Size(785, 321);
        textBoxResponse.TabIndex = 1;
        // 
        // buttonSendPrompt
        // 
        buttonSendPrompt.Location = new System.Drawing.Point(240, 206);
        buttonSendPrompt.Name = "buttonSendPrompt";
        buttonSendPrompt.Size = new System.Drawing.Size(127, 50);
        buttonSendPrompt.TabIndex = 2;
        buttonSendPrompt.Text = "Send Prompt";
        buttonSendPrompt.UseVisualStyleBackColor = true;
        buttonSendPrompt.Click += buttonSendPrompt_Click_1;
        // 
        // buttonDownloadResponse
        // 
        buttonDownloadResponse.Location = new System.Drawing.Point(456, 208);
        buttonDownloadResponse.Name = "buttonDownloadResponse";
        buttonDownloadResponse.Size = new System.Drawing.Size(127, 48);
        buttonDownloadResponse.TabIndex = 3;
        buttonDownloadResponse.Text = "Download";
        buttonDownloadResponse.UseVisualStyleBackColor = true;
        buttonDownloadResponse.Click += buttonDownloadResponse_Click;
        // 
        // textBoxPrompt
        // 
        textBoxPrompt.Location = new System.Drawing.Point(19, 56);
        textBoxPrompt.Multiline = true;
        textBoxPrompt.Name = "textBoxPrompt";
        textBoxPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBoxPrompt.Size = new System.Drawing.Size(785, 130);
        textBoxPrompt.TabIndex = 4;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(19, 25);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(145, 28);
        label1.TabIndex = 5;
        label1.Text = "Prompt";
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(26, 246);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(112, 30);
        label2.TabIndex = 6;
        label2.Text = "Response";
        // 
        // OpenAIForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(821, 616);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(textBoxPrompt);
        Controls.Add(buttonDownloadResponse);
        Controls.Add(buttonSendPrompt);
        Controls.Add(textBoxResponse);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "OpenAIForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.TextBox textBoxPrompt;

    private System.Windows.Forms.Button buttonDownloadResponse;

    private System.Windows.Forms.Button buttonSendPrompt;

    private System.Windows.Forms.TextBox textBoxResponse;

    #endregion
}