using System.Diagnostics;

namespace ChatToMarkdown;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void ConvertButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(JsonTextBox.Text))
        {
            MessageBox.Show("Json is empty", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        }

        var dialog = new SaveFileDialog();
        dialog.Filter = "Markdown Files (*.md)|*.md";

        if (dialog.ShowDialog() != DialogResult.OK) return;

        try
        {
            var messages = ChatConverter.ExtractMessages(JsonTextBox.Text);
            var markdown = ChatConverter.ConvertToMarkdown(messages);

            File.WriteAllText(dialog.FileName, markdown); 
            
            try
            {
                // Launch the file with its default program
                Process.Start(new ProcessStartInfo
                {
                    FileName = dialog.FileName,
                    UseShellExecute = true // Important for opening in default associated application
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.GetType().Name}\n{ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } 
    }
}