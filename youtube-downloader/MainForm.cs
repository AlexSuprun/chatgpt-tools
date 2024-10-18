namespace YoutubeDownloader;

public partial class MainForm : Form
{
    private readonly AudioTranscriber _audioTranscriber = new(Program.OpenAiConfig.ApiKey); 
    private readonly VideoProcessor _processor = new();
    public MainForm()
    {
        InitializeComponent();
    }
    private void LogToWindow(string message)
    {
        Console.WriteLine(message);
    }

    private async void DownloadButton_Click(object sender, EventArgs e)
    {
        StatusLabel.Text = "Initializing"; 
        
        var videoInfo = await _processor.GetVideoInfo(textBox1.Text);
        var sanitizedTitle = string.Join("_", videoInfo.Title.Split(Path.GetInvalidFileNameChars()));

        var saveDialog = new SaveFileDialog();
        saveDialog.Filter = "mp4 files (*.mp4)|*.mp4";
        saveDialog.FileName = $"{sanitizedTitle}.mp4";


        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            var downloadProgress = new Progress<double>(value =>
            {
                ProcessingProgressBar.Value = (int)(value * 100);
            });
            
            var mergeProgress = new Progress<double>(value =>
            {
                ProcessingProgressBar.Value = (int)value;
            });
            
            StatusLabel.Text = "Downloading";
            
            var videoParts =  await _processor.DownloadAsync(textBox1.Text, downloadProgress);

            StatusLabel.Text = "Transcribing";
            var subtitles = await _audioTranscriber.TranscribeToSrtAsync(videoParts.AudioFile);

            if (!string.IsNullOrEmpty(subtitles))
            {
                var outputdDirectoryName = Path.GetDirectoryName(saveDialog.FileName);
                var subtitlesFile = Path.Combine(outputdDirectoryName,
                    $"{Path.GetFileNameWithoutExtension(saveDialog.FileName)}.srt"); 
                
                await using var writer = new StreamWriter(subtitlesFile);
                await writer.WriteAsync(subtitles);
            }

            StatusLabel.Text = "Merging";
            await FFmpegMerger.Merge(videoParts.AudioFile, videoParts.VideoFile, saveDialog.FileName, mergeProgress);
            File.Delete(videoParts.AudioFile);
            File.Delete(videoParts.VideoFile);
            
            StatusLabel.Text = "Completed"; 
        }
    }
}