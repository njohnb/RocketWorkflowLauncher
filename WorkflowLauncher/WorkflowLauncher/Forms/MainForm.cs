using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Extensions.Configuration;

namespace WorkflowLauncher
{
    public partial class MainForm : Form
    {
        private WorkflowProfile currentProfile;
        private bool allowClose = false;
        public MainForm()
        {
            InitializeComponent();

            SetupTrayMenu();
            
            comboBoxProfiles.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxProfiles.DrawItem += comboBoxProfiles_DrawItem;
            
            LoadProfiles();
            
            string defaultProfile = SettingsManager.GetDefaultProfile();
            if (!string.IsNullOrEmpty(defaultProfile))
            {
                int index = comboBoxProfiles.Items.IndexOf(defaultProfile);
                if (index >= 0)
                    comboBoxProfiles.SelectedIndex = index;
                currentProfile = WorkflowManager.LoadProfile(defaultProfile);
                RefreshItemList();
            } else if (comboBoxProfiles.Items.Count > 0)
            {
                comboBoxProfiles.SelectedIndex = 0;

                var profileName = comboBoxProfiles.Items[0].ToString().Replace(" (default)", "");
                currentProfile = WorkflowManager.LoadProfile(profileName);
                RefreshItemList();
            }
            
            
        }

        private void LoadProfiles()
        {
            comboBoxProfiles.SelectedIndexChanged -= comboBoxProfiles_SelectedIndexChanged;
            
            comboBoxProfiles.Items.Clear();
            comboBoxProfiles.Text = "";
            
            var profiles = WorkflowManager.GetAvailableProfiles();
            string defaultProfile = SettingsManager.GetDefaultProfile();
            int selectedIndex = -1;

            for (int i = 0; i < profiles.Count; i++)
            {
                string profileName = profiles[i];

                if (profileName == defaultProfile)
                {
                    comboBoxProfiles.Items.Add(profileName + " (default)");
                    selectedIndex = i;
                }
                else
                {
                    comboBoxProfiles.Items.Add(profileName);
                }
            }

            if (profiles.Count > 0)
            {
                comboBoxProfiles.SelectedIndex = (selectedIndex >= 0) ? selectedIndex : 0;
            }
            else
            {
                comboBoxProfiles.SelectedIndex = -1;
                listBoxItems.Items.Clear();
                currentProfile = null;
            }
            
            comboBoxProfiles.SelectedIndexChanged += comboBoxProfiles_SelectedIndexChanged;
        }

        private void RefreshItemList()
        {
            listBoxItems.Items.Clear();
            foreach (var item in currentProfile.Items)
            {
                listBoxItems.Items.Add($"[{item.Type}] {item.Name} - {item.PathOrURL}");
            }
        }
        
        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var profileName = comboBoxProfiles.SelectedItem.ToString().Replace(" (default)", "");
            currentProfile = WorkflowManager.LoadProfile(profileName);
            RefreshItemList();
            checkBoxOnStartup.Checked = currentProfile.bStartOnStartup;
        }

        private void buttonRunAll_Click(object sender, EventArgs e)
        {
            foreach (var item in currentProfile.Items)
                Launcher.Launch(item);
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            var form = new AddEditItemForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                currentProfile.Items.Add(form.Result);
                WorkflowManager.SaveProfile(currentProfile);
                RefreshItemList();
            }
        }

        private void buttonSaveProfile_Click(object sender, EventArgs e)
        {
            if (currentProfile != null)
            {
                WorkflowManager.SaveProfile(currentProfile);
                MessageBox.Show("Profile saved successfully");
            }
            else
            {
                MessageBox.Show("Profile could not be saved");
            }
        }

        private void buttonNewProfile_Click(object sender, EventArgs e)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox("Enter profile name:", "New Profile");
            if (!string.IsNullOrWhiteSpace(input))
            {
                currentProfile = new WorkflowProfile { ProfileName = input };
                WorkflowManager.SaveProfile(currentProfile);
                LoadProfiles();
                comboBoxProfiles.SelectedItem = input;
                SetupTrayMenu();
            }
        }

        private void listBoxItems_DoubleClick(object sender, EventArgs e)
        {
            int index = listBoxItems.SelectedIndex;
            if (index < 0 || index >= currentProfile.Items.Count) return;
            
            var itemToEdit = currentProfile.Items[index];
            var form = new AddEditItemForm(itemToEdit);

            if (form.ShowDialog() == DialogResult.OK)
            {
                currentProfile.Items[index] = form.Result;
                WorkflowManager.SaveProfile(currentProfile);
                RefreshItemList();
            }
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            int index = listBoxItems.SelectedIndex;
            if(index < 0 || index >= currentProfile.Items.Count) return;

            var result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                currentProfile.Items.RemoveAt(index);
                WorkflowManager.SaveProfile(currentProfile);
                RefreshItemList();
            }
        }

        private void buttonDeleteProfile_Click(object sender, EventArgs e)
        {
            if (comboBoxProfiles.SelectedItem == null)
            {
                MessageBox.Show("Please select a profile to delete.");
                return;
            }

            string profileName = comboBoxProfiles.SelectedItem.ToString().Replace(" (default)", "");
            string path = Path.Combine(WorkflowManager.ProfilesDir, profileName + ".json");

            var confirm = MessageBox.Show($"Are you sure you want to delete profile \"{profileName}\"?",
                "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                // check if deleting default profile
                string defaultProfile = SettingsManager.GetDefaultProfile();
                if (profileName == defaultProfile)
                {
                    SettingsManager.SetDefaultProfile("");
                }
                
                if(File.Exists(path))
                    File.Delete(path);
                
                MessageBox.Show($"Profile \"{profileName}\" deleted.");

                int deletedIndex = comboBoxProfiles.SelectedIndex;
                
                LoadProfiles();
                
                var profiles = WorkflowManager.GetAvailableProfiles();
                if (profiles.Count > 0)
                {
                    int nextIndex = Math.Min(deletedIndex, profiles.Count - 1);
                    comboBoxProfiles.SelectedIndex = nextIndex;
                    profileName = comboBoxProfiles.SelectedItem.ToString().Replace(" (default)", ""); 
                    currentProfile = WorkflowManager.LoadProfile(profileName);
                    checkBoxOnStartup.Checked = currentProfile.bStartOnStartup;
                    RefreshItemList();
                }
                else
                {
                    listBoxItems.Items.Clear();
                    currentProfile = null;
                }
                
                SetupTrayMenu();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to delete profile: " + exception.Message);
            }
        }

        private void buttonSetDefaultProfile_Click(object sender, EventArgs e)
        {
            if (comboBoxProfiles.SelectedItem == null)
            {
                MessageBox.Show("Please select a profile to set.");
                return;
            }

            string selectedProfile = comboBoxProfiles.SelectedItem.ToString().Replace(" (default)", "");
            SettingsManager.SetDefaultProfile(selectedProfile);
            MessageBox.Show($"\"{selectedProfile}\" set as default profile.");
            
            LoadProfiles();
            for (int i = 0; i < comboBoxProfiles.Items.Count; i++)
            {
                if (comboBoxProfiles.Items[i].ToString().Contains(selectedProfile))
                {
                    comboBoxProfiles.SelectedIndex = i;
                    break;
                }
            }
        }

        private void comboBoxProfiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            
            e.DrawBackground();

            string itemText = comboBoxProfiles.Items[e.Index].ToString();
            string defaultProfile = SettingsManager.GetDefaultProfile();
            string cleanProfileName = itemText.Replace(" (default)", "");
            
            bool isDefault = cleanProfileName == defaultProfile;

            Font font = isDefault ? new Font(e.Font, FontStyle.Bold) : e.Font;
            Brush brush = isDefault ? Brushes.DarkGreen : Brushes.Black;
            e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);;
            e.DrawFocusRectangle();
        }

        private void SetupTrayMenu()
        {
            contextMenuStrip1.Items.Clear();
            
            // Launch default
            var launchDefault = new ToolStripMenuItem("Launch Default Profile", null, (s, e) => LaunchDefaultProfile());
            contextMenuStrip1.Items.Add(launchDefault);
            
            // separator
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            
            // Profile List
            var profiles = WorkflowManager.GetAvailableProfiles();
            foreach (var profile in profiles)
            {
                var item = new ToolStripMenuItem($"Launch {profile}", null, (s, e) => LaunchProfile(profile));
                contextMenuStrip1.Items.Add(item);
            }

            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            
            // open main window
            contextMenuStrip1.Items.Add(new ToolStripMenuItem("Open Launcher", null, (s, e) => ShowMainWindow()));
            
            // exit
            contextMenuStrip1.Items.Add(new ToolStripMenuItem("Exit", null, (s, e) =>
            {
                allowClose = true;
                Application.Exit();
            }));

        }

        private void LaunchDefaultProfile()
        {
            string defaultProfile = SettingsManager.GetDefaultProfile();
            if (!string.IsNullOrEmpty(defaultProfile))
            {
                var profile = WorkflowManager.LoadProfile(defaultProfile);
                foreach (var item in profile.Items)
                    Launcher.Launch(item);
            }
            else
            {
                MessageBox.Show("No default profile selected.");
            }
        }

        private void LaunchProfile(string profileName)
        {
            var profile = WorkflowManager.LoadProfile(profileName);
            foreach (var item in profile.Items)
                Launcher.Launch(item);
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }
        
        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (!allowClose)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowMainWindow();
            }
        }

        private void buttonMakeShortcut_Click(object sender, EventArgs e)
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                currentProfile.ProfileName + ".lnk");
            string exePath = Application.ExecutablePath;
            string arguments = $"--profile \"{currentProfile.ProfileName}\"";
            ShortcutHelper.CreateShortcut(shortcutPath, exePath, arguments);
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (currentProfile == null) return;

            foreach (var name in WorkflowManager.GetAvailableProfiles())
            {
                var profile = WorkflowManager.LoadProfile(name);
                profile.bStartOnStartup = false;
                WorkflowManager.SaveProfile(profile);
            }

            currentProfile.bStartOnStartup = checkBoxOnStartup.Checked;
            WorkflowManager.SaveProfile(currentProfile);
        }
    }
}