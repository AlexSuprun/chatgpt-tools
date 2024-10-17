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
            return; 
        }


        try
        {
            var chat = ChatConverter.ExtractMessages(JsonTextBox.Text);
            var markdown = ChatConverter.ConvertToMarkdown(chat.Conversation, chat.Messages);

            var dialog = new SaveFileDialog();
            dialog.Filter = "Markdown Files (*.md)|*.md";
            dialog.FileName = $"{chat.Conversation.Title}.md";

            if (dialog.ShowDialog(this) != DialogResult.OK) return;


            File.WriteAllText(dialog.FileName, markdown);

            if (OpenCheckBox.Checked)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = dialog.FileName,
                    UseShellExecute = true // Important for opening in default associated application
                });
            }

            if (SaveJsonCheckBox.Checked)
            {
                SaveJson(chat);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.GetType().Name}\n{ex.Message}", "Exception", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void SaveJson((Conversation Conversation, List<MessageNode> Messages) chat)
    {
        var dialog = new SaveFileDialog();
        dialog.Filter = "JSON Files(*.json)|*.json";
        dialog.FileName = $"{chat.Conversation.Title}.json";

        if(dialog.ShowDialog(this) != DialogResult.OK)
        {
            File.WriteAllText(dialog.FileName, JsonTextBox.Text);
        }
    }
}