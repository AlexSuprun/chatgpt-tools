using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitlesGenerator
{
    public partial class MainForm : Form
    {
        private readonly AudioTranscriber _audioTranscriber = new AudioTranscriber(Program.OpenAiConfig.ApiKey); 

        public MainForm()
        {
            InitializeComponent();
        }

        private async void GenerateSubtitles_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "mp4 files (*.mp4)|*.mp4";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var videoPath = openFileDialog.FileName;
                    var outputPath = $"{videoPath}.srt";

                    var subtitles = await _audioTranscriber.TranscribeToSrtAsync(openFileDialog.FileName);
                    var subtitlesFileName = Path.Combine(Path.GetDirectoryName(openFileDialog.FileName), Path.GetFileNameWithoutExtension(openFileDialog.FileName) + ".srt");

                    using var writer = new StreamWriter(subtitlesFileName);
                    await writer.WriteAsync(subtitles);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType().Name}-> {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }
    }
}
