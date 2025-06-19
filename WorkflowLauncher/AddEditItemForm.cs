using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace WorkflowLauncher
{
    public partial class AddEditItemForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WorkflowItem Result { get; private set; }

        public AddEditItemForm(WorkflowItem existingItem = null)
        {
            InitializeComponent();
            comboBoxType.DataSource = Enum.GetValues(typeof(LaunchType));

            if (existingItem != null)
            {
                textBoxName.Text = existingItem.Name;
                textBoxPath.Text = existingItem.PathOrURL;
                comboBoxType.SelectedItem = existingItem.Type;
                checkBoxEnabled.Checked = existingItem.Enabled;
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                textBoxPath.Text = ofd.FileName;
        }
        
        private void buttonOK_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxPath.Text))
            {
                MessageBox.Show("Please enter a name or path.");
                return;
            }
            if (comboBoxType.SelectedItem == null)
            {
                MessageBox.Show("Please select a type");
                return;
            }
            
            Result = new WorkflowItem
            {
                Name = textBoxName.Text,
                PathOrURL = textBoxPath.Text,
                Arguments = textBoxArguments.Text,
                Type = (LaunchType)comboBoxType.SelectedItem,
                Enabled = checkBoxEnabled.Checked
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}