using System.Diagnostics;
using System.Runtime;

namespace TimeOs2Srt
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void ConvertToSrt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TranscriptTextBox.Text))
            {
                MessageBox.Show("Transcript is empty", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            var srt = TranscriptConverter.ConvertToSrt(TranscriptTextBox.Text);

            var openVideoDialog = new OpenFileDialog(); 
            openVideoDialog.Filter = "mp4 files (*.mp4)|*.mp4";

            if (openVideoDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var directory = Path.GetDirectoryName(openVideoDialog.FileName);
                    var fileName = $"{Path.GetFileNameWithoutExtension(openVideoDialog.FileName)}.srt" ;

                    var srtFile = Path.Combine(directory, fileName);

                    if (File.Exists(srtFile))
                    {
                        File.Delete(srtFile);
                    }

                    using var writer = new StreamWriter(srtFile); 
                    writer.Write(srt);
                
                    if (OpenCheckBox.Checked)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = openVideoDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType().Name}:\n{ex.Message}", "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
