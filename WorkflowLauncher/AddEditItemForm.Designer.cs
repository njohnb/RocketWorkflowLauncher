using System.ComponentModel;

namespace WorkflowLauncher;

partial class AddEditItemForm
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
        textBoxName = new System.Windows.Forms.TextBox();
        comboBoxType = new System.Windows.Forms.ComboBox();
        textBoxPath = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        checkBoxEnabled = new System.Windows.Forms.CheckBox();
        buttonOK = new System.Windows.Forms.Button();
        buttonCancel = new System.Windows.Forms.Button();
        buttonBrowse = new System.Windows.Forms.Button();
        textBoxArguments = new System.Windows.Forms.TextBox();
        label4 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // textBoxName
        // 
        textBoxName.AccessibleName = "";
        textBoxName.Location = new System.Drawing.Point(149, 37);
        textBoxName.Name = "textBoxName";
        textBoxName.Size = new System.Drawing.Size(154, 23);
        textBoxName.TabIndex = 0;
        // 
        // comboBoxType
        // 
        comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxType.FormattingEnabled = true;
        comboBoxType.Location = new System.Drawing.Point(149, 66);
        comboBoxType.Name = "comboBoxType";
        comboBoxType.Size = new System.Drawing.Size(154, 23);
        comboBoxType.TabIndex = 1;
        comboBoxType.Tag = "";
        // 
        // textBoxPath
        // 
        textBoxPath.Location = new System.Drawing.Point(149, 100);
        textBoxPath.Name = "textBoxPath";
        textBoxPath.Size = new System.Drawing.Size(266, 23);
        textBoxPath.TabIndex = 2;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(43, 37);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(100, 23);
        label1.TabIndex = 3;
        label1.Text = "Name";
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(43, 66);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(100, 23);
        label2.TabIndex = 4;
        label2.Text = "Type";
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(43, 100);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(100, 23);
        label3.TabIndex = 5;
        label3.Text = "Path or URL";
        // 
        // checkBoxEnabled
        // 
        checkBoxEnabled.Checked = true;
        checkBoxEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
        checkBoxEnabled.Location = new System.Drawing.Point(221, 180);
        checkBoxEnabled.Name = "checkBoxEnabled";
        checkBoxEnabled.Size = new System.Drawing.Size(104, 24);
        checkBoxEnabled.TabIndex = 6;
        checkBoxEnabled.Text = "Enabled";
        checkBoxEnabled.UseVisualStyleBackColor = true;
        // 
        // buttonOK
        // 
        buttonOK.Location = new System.Drawing.Point(187, 210);
        buttonOK.Name = "buttonOK";
        buttonOK.Size = new System.Drawing.Size(75, 23);
        buttonOK.TabIndex = 7;
        buttonOK.Text = "OK";
        buttonOK.UseVisualStyleBackColor = true;
        buttonOK.Click += buttonOK_Click;
        // 
        // buttonCancel
        // 
        buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        buttonCancel.Location = new System.Drawing.Point(268, 210);
        buttonCancel.Name = "buttonCancel";
        buttonCancel.Size = new System.Drawing.Size(75, 23);
        buttonCancel.TabIndex = 8;
        buttonCancel.Text = "Cancel";
        buttonCancel.UseVisualStyleBackColor = true;
        buttonCancel.Click += buttonCancel_Click;
        // 
        // buttonBrowse
        // 
        buttonBrowse.Location = new System.Drawing.Point(421, 100);
        buttonBrowse.Name = "buttonBrowse";
        buttonBrowse.Size = new System.Drawing.Size(75, 23);
        buttonBrowse.TabIndex = 9;
        buttonBrowse.Text = "Browse";
        buttonBrowse.UseVisualStyleBackColor = true;
        buttonBrowse.Click += buttonBrowse_Click;
        // 
        // textBoxArguments
        // 
        textBoxArguments.Location = new System.Drawing.Point(149, 129);
        textBoxArguments.Name = "textBoxArguments";
        textBoxArguments.Size = new System.Drawing.Size(266, 23);
        textBoxArguments.TabIndex = 10;
        // 
        // label4
        // 
        label4.Location = new System.Drawing.Point(43, 129);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(100, 23);
        label4.TabIndex = 11;
        label4.Text = "Arguments";
        // 
        // AddEditItemForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(555, 300);
        Controls.Add(label4);
        Controls.Add(textBoxArguments);
        Controls.Add(buttonBrowse);
        Controls.Add(buttonCancel);
        Controls.Add(buttonOK);
        Controls.Add(checkBoxEnabled);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(textBoxPath);
        Controls.Add(comboBoxType);
        Controls.Add(textBoxName);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "AddEditItemForm";
        TopMost = true;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox textBoxArguments;
    private System.Windows.Forms.Label label4;

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox checkBoxEnabled;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonBrowse;

    private System.Windows.Forms.TextBox textBoxPath;

    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.ComboBox comboBoxType;

    #endregion
}