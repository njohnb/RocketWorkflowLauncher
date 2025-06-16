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
        SuspendLayout();
        // 
        // comboBoxProfiles
        // 
        comboBoxProfiles.FormattingEnabled = true;
        comboBoxProfiles.Location = new System.Drawing.Point(9, 13);
        comboBoxProfiles.Name = "comboBoxProfiles";
        comboBoxProfiles.Size = new System.Drawing.Size(200, 23);
        comboBoxProfiles.TabIndex = 0;
        comboBoxProfiles.SelectedIndexChanged += comboBoxProfiles_SelectedIndexChanged;
        // 
        // listBoxItems
        // 
        listBoxItems.FormattingEnabled = true;
        listBoxItems.Location = new System.Drawing.Point(9, 42);
        listBoxItems.Name = "listBoxItems";
        listBoxItems.Size = new System.Drawing.Size(300, 199);
        listBoxItems.TabIndex = 1;
        listBoxItems.DoubleClick += listBoxItems_DoubleClick;
        // 
        // buttonRunAll
        // 
        buttonRunAll.Location = new System.Drawing.Point(315, 42);
        buttonRunAll.Name = "buttonRunAll";
        buttonRunAll.Size = new System.Drawing.Size(166, 23);
        buttonRunAll.TabIndex = 2;
        buttonRunAll.Text = "Run All";
        buttonRunAll.UseVisualStyleBackColor = true;
        buttonRunAll.Click += buttonRunAll_Click;
        // 
        // buttonAddItem
        // 
        buttonAddItem.Location = new System.Drawing.Point(315, 71);
        buttonAddItem.Name = "buttonAddItem";
        buttonAddItem.Size = new System.Drawing.Size(166, 23);
        buttonAddItem.TabIndex = 3;
        buttonAddItem.Text = "Add Item";
        buttonAddItem.UseVisualStyleBackColor = true;
        buttonAddItem.Click += buttonAddItem_Click;
        // 
        // buttonSaveProfile
        // 
        buttonSaveProfile.Location = new System.Drawing.Point(315, 158);
        buttonSaveProfile.Name = "buttonSaveProfile";
        buttonSaveProfile.Size = new System.Drawing.Size(166, 23);
        buttonSaveProfile.TabIndex = 4;
        buttonSaveProfile.Text = "Save Profile";
        buttonSaveProfile.UseVisualStyleBackColor = true;
        buttonSaveProfile.Click += buttonSaveProfile_Click;
        // 
        // buttonNewProfile
        // 
        buttonNewProfile.Location = new System.Drawing.Point(315, 129);
        buttonNewProfile.Name = "buttonNewProfile";
        buttonNewProfile.Size = new System.Drawing.Size(166, 23);
        buttonNewProfile.TabIndex = 5;
        buttonNewProfile.Text = "New Profile";
        buttonNewProfile.UseVisualStyleBackColor = true;
        buttonNewProfile.Click += buttonNewProfile_Click;
        // 
        // buttonDeleteItem
        // 
        buttonDeleteItem.Location = new System.Drawing.Point(315, 100);
        buttonDeleteItem.Name = "buttonDeleteItem";
        buttonDeleteItem.Size = new System.Drawing.Size(166, 23);
        buttonDeleteItem.TabIndex = 6;
        buttonDeleteItem.Text = "Delete Item";
        buttonDeleteItem.UseVisualStyleBackColor = true;
        buttonDeleteItem.Click += buttonDeleteItem_Click;
        // 
        // buttonDeleteProfile
        // 
        buttonDeleteProfile.Location = new System.Drawing.Point(315, 187);
        buttonDeleteProfile.Name = "buttonDeleteProfile";
        buttonDeleteProfile.Size = new System.Drawing.Size(166, 23);
        buttonDeleteProfile.TabIndex = 7;
        buttonDeleteProfile.Text = "Delete Profile";
        buttonDeleteProfile.UseVisualStyleBackColor = true;
        buttonDeleteProfile.Click += buttonDeleteProfile_Click;
        // 
        // buttonSetDefaultProfile
        // 
        buttonSetDefaultProfile.Location = new System.Drawing.Point(315, 216);
        buttonSetDefaultProfile.Name = "buttonSetDefaultProfile";
        buttonSetDefaultProfile.Size = new System.Drawing.Size(166, 23);
        buttonSetDefaultProfile.TabIndex = 8;
        buttonSetDefaultProfile.Text = "Set as Default";
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
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(509, 261);
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
        MaximizeBox = false;
        SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Rocket Workflow Launcher";
        FormClosing += MainForm_Closing;
        ResumeLayout(false);
    }

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