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

            if (openFileDialog.ShowDialog() == DialogResult.OK)
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

        private async void TranslateSubtitlesButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Subtitles files (*.srt)|*.srt";

            var language = "he"; 

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            { 
                var sourceSubtitles = SubtitleProcessor.ParseSrtFile(openFileDialog.FileName);


                //var destSubtitles = await TranslationService.TranslateBatchAsync(sourceSubtitles.Take(13).ToList());// new List<Subtitle>(sourceSubtitles.Count);
                
                var batchSize = 13; // Adjust based on context needs and token limits
                var translatedSubtitles = new List<Subtitle>();
                for (int i = 0; i < sourceSubtitles.Count; i += batchSize)
                {
                    var batch = sourceSubtitles.GetRange(i, Math.Min(batchSize, sourceSubtitles.Count - i));
                    var translatedBatch = await TranslationService.TranslateBatchAsync(batch);
                    translatedSubtitles.AddRange(translatedBatch);
                }
                
                var fileName = Path.Combine(Path.GetDirectoryName(openFileDialog.FileName), 
                    Path.GetFileNameWithoutExtension(openFileDialog.FileName)+"."+language+".srt");
                using var writer = new StreamWriter(fileName);

                foreach (var subtitle in translatedSubtitles)
                {
                    await writer.WriteLineAsync(subtitle.Index.ToString());
                    await writer.WriteLineAsync($"{subtitle.StartTime.ToString(@"hh\:mm\:ss\,fff")} --> {subtitle.EndTime.ToString(@"hh\:mm\:ss\,fff")}");
                    await writer.WriteLineAsync(subtitle.Text);
                    await writer.WriteLineAsync();
                }
            }
        }
    }
}
