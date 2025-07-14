namespace WorkflowLauncher;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        comboBoxProfiles = new System.Windows.Forms.ComboBox();
        listBoxItems = new System.Windows.Forms.ListBox();
        buttonRunAll = new System.Windows.Forms.Button();
        buttonAddItem = new System.Windows.Forms.Button();
        buttonSaveProfile = new System.Windows.Forms.Button();
        buttonNewProfile = new System.Windows.Forms.Button();
        buttonDeleteItem = new System.Windows.Forms.Button();
        buttonDeleteProfile = new System.Windows.Forms.Button();
        buttonSetDefaultProfile = new System.Windows.Forms.Button();
        notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
        contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
        buttonMakeShortcut = new System.Windows.Forms.Button();
        checkBoxOnStartup = new System.Windows.Forms.CheckBox();
        label1 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // comboBoxProfiles
        // 
        comboBoxProfiles.FormattingEnabled = true;
        comboBoxProfiles.Location = new System.Drawing.Point(12, 28);
        comboBoxProfiles.Name = "comboBoxProfiles";
        comboBoxProfiles.Size = new System.Drawing.Size(248, 23);
        comboBoxProfiles.TabIndex = 0;
        comboBoxProfiles.SelectedIndexChanged += comboBoxProfiles_SelectedIndexChanged;
        // 
        // listBoxItems
        // 
        listBoxItems.FormattingEnabled = true;
        listBoxItems.Location = new System.Drawing.Point(12, 94);
        listBoxItems.Name = "listBoxItems";
        listBoxItems.Size = new System.Drawing.Size(654, 169);
        listBoxItems.TabIndex = 1;
        listBoxItems.DoubleClick += listBoxItems_DoubleClick;
        // 
        // buttonRunAll
        // 
        buttonRunAll.Location = new System.Drawing.Point(266, 10);
        buttonRunAll.Name = "buttonRunAll";
        buttonRunAll.Size = new System.Drawing.Size(175, 35);
        buttonRunAll.TabIndex = 2;
        buttonRunAll.Text = "Run Profile";
        buttonRunAll.UseVisualStyleBackColor = true;
        buttonRunAll.Click += buttonRunAll_Click;
        // 
        // buttonAddItem
        // 
        buttonAddItem.Location = new System.Drawing.Point(310, 284);
        buttonAddItem.Name = "buttonAddItem";
        buttonAddItem.Size = new System.Drawing.Size(175, 35);
        buttonAddItem.TabIndex = 3;
        buttonAddItem.Text = "Add Item";
        buttonAddItem.UseVisualStyleBackColor = true;
        buttonAddItem.Click += buttonAddItem_Click;
        // 
        // buttonSaveProfile
        // 
        buttonSaveProfile.Location = new System.Drawing.Point(462, 12);
        buttonSaveProfile.Name = "buttonSaveProfile";
        buttonSaveProfile.Size = new System.Drawing.Size(175, 35);
        buttonSaveProfile.TabIndex = 4;
        buttonSaveProfile.Text = "Save Profile";
        buttonSaveProfile.UseVisualStyleBackColor = true;
        buttonSaveProfile.Click += buttonSaveProfile_Click;
        // 
        // buttonNewProfile
        // 
        buttonNewProfile.Location = new System.Drawing.Point(266, 51);
        buttonNewProfile.Name = "buttonNewProfile";
        buttonNewProfile.Size = new System.Drawing.Size(175, 35);
        buttonNewProfile.TabIndex = 5;
        buttonNewProfile.Text = "New Profile";
        buttonNewProfile.UseVisualStyleBackColor = true;
        buttonNewProfile.Click += buttonNewProfile_Click;
        // 
        // buttonDeleteItem
        // 
        buttonDeleteItem.Location = new System.Drawing.Point(491, 284);
        buttonDeleteItem.Name = "buttonDeleteItem";
        buttonDeleteItem.Size = new System.Drawing.Size(175, 35);
        buttonDeleteItem.TabIndex = 6;
        buttonDeleteItem.Text = "Delete Item";
        buttonDeleteItem.UseVisualStyleBackColor = true;
        buttonDeleteItem.Click += buttonDeleteItem_Click;
        // 
        // buttonDeleteProfile
        // 
        buttonDeleteProfile.Location = new System.Drawing.Point(462, 53);
        buttonDeleteProfile.Name = "buttonDeleteProfile";
        buttonDeleteProfile.Size = new System.Drawing.Size(175, 35);
        buttonDeleteProfile.TabIndex = 7;
        buttonDeleteProfile.Text = "Delete Profile";
        buttonDeleteProfile.UseVisualStyleBackColor = true;
        buttonDeleteProfile.Click += buttonDeleteProfile_Click;
        // 
        // buttonSetDefaultProfile
        // 
        buttonSetDefaultProfile.Location = new System.Drawing.Point(17, 279);
        buttonSetDefaultProfile.Name = "buttonSetDefaultProfile";
        buttonSetDefaultProfile.Size = new System.Drawing.Size(250, 40);
        buttonSetDefaultProfile.TabIndex = 8;
        buttonSetDefaultProfile.Text = "Set Profile as Default";
        buttonSetDefaultProfile.UseVisualStyleBackColor = true;
        buttonSetDefaultProfile.Click += buttonSetDefaultProfile_Click;
        // 
        // notifyIcon1
        // 
        notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        notifyIcon1.Icon = ((System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon"));
        notifyIcon1.Text = "Workflow Launcher";
        notifyIcon1.Visible = true;
        notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new System.Drawing.Size(181, 26);
        // 
        // buttonMakeShortcut
        // 
        buttonMakeShortcut.Location = new System.Drawing.Point(17, 325);
        buttonMakeShortcut.Name = "buttonMakeShortcut";
        buttonMakeShortcut.Size = new System.Drawing.Size(250, 40);
        buttonMakeShortcut.TabIndex = 9;
        buttonMakeShortcut.Text = "Make Desktop Shortcut";
        buttonMakeShortcut.UseVisualStyleBackColor = true;
        buttonMakeShortcut.Click += buttonMakeShortcut_Click;
        // 
        // checkBoxOnStartup
        // 
        checkBoxOnStartup.Location = new System.Drawing.Point(327, 345);
        checkBoxOnStartup.Name = "checkBoxOnStartup";
        checkBoxOnStartup.Size = new System.Drawing.Size(21, 20);
        checkBoxOnStartup.TabIndex = 10;
        checkBoxOnStartup.Text = "checkBox1";
        checkBoxOnStartup.UseVisualStyleBackColor = true;
        checkBoxOnStartup.Click += checkBox1_Click;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(354, 342);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(312, 27);
        label1.TabIndex = 11;
        label1.Text = "Run profile on application startup";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(678, 394);
        Controls.Add(label1);
        Controls.Add(checkBoxOnStartup);
        Controls.Add(buttonMakeShortcut);
        Controls.Add(buttonSetDefaultProfile);
        Controls.Add(buttonDeleteProfile);
        Controls.Add(buttonDeleteItem);
        Controls.Add(buttonNewProfile);
        Controls.Add(buttonSaveProfile);
        Controls.Add(buttonAddItem);
        Controls.Add(buttonRunAll);
        Controls.Add(listBoxItems);
        Controls.Add(comboBoxProfiles);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
        MaximizeBox = false;
        SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Rocket Workflow Launcher";
        FormClosing += MainForm_Closing;
        ResumeLayout(false);
    }

    private System.Windows.Forms.CheckBox checkBoxOnStartup;
    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button buttonMakeShortcut;

    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

    private System.Windows.Forms.NotifyIcon notifyIcon1;

    private System.Windows.Forms.Button buttonSetDefaultProfile;

    private System.Windows.Forms.Button buttonDeleteProfile;

    private System.Windows.Forms.Button buttonDeleteItem;

    private System.Windows.Forms.ListBox listBoxItems;
    private System.Windows.Forms.Button buttonRunAll;
    private System.Windows.Forms.Button buttonAddItem;
    private System.Windows.Forms.Button buttonSaveProfile;
    private System.Windows.Forms.Button buttonNewProfile;

    private System.Windows.Forms.ComboBox comboBoxProfiles;

    #endregion
}