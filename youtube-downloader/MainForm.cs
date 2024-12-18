using System.Diagnostics;
using FFMpegCore;

namespace YoutubeDownloader;

public partial class MainForm : Form
{
    private readonly VideoProcessor _processor = new();
    public MainForm()
    {
        InitializeComponent();
    }
    private void LogToWindow(string message)
    {
        Console.WriteLine(message);
    }

    private async void DownloadVideoButton_Click(object sender, EventArgs e)
    {
        StatusLabel.Text = "Initializing";

        var videoInfo = await _processor.GetVideoInfo(UrlTextBox.Text);
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

            var videoParts = await _processor.DownloadAsync(UrlTextBox.Text, downloadProgress);

            StatusLabel.Text = "Merging";
            await FFmpegMerger.Merge(videoParts.AudioFile, videoParts.VideoFile, saveDialog.FileName, mergeProgress);
            File.Delete(videoParts.AudioFile);
            File.Delete(videoParts.VideoFile);

            StatusLabel.Text = "Completed";
        }
    }

    static (TimeSpan duration, long fileSize) GetVideoInfo(string filePath)
    {
        // Get video duration using ffprobe
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffprobe",
                Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{filePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string durationStr = process.StandardOutput.ReadLine();
        process.WaitForExit();

        // Get video file size
        long fileSize = new FileInfo(filePath).Length;

        return (TimeSpan.FromSeconds(double.Parse(durationStr)), fileSize);
    }
    public int CalculateChunkDuration(long fileSizeInBytes, TimeSpan videoDuration)
    {
        const long maxChunkSizeInBytes = 25 * 1024 * 1024; // 25 MB in bytes
        double totalDurationInSeconds = videoDuration.TotalSeconds;

        // Calculate the bitrate of the file in bytes per second
        double fileBitrate = fileSizeInBytes / totalDurationInSeconds;

        // Calculate the maximum duration of a chunk in seconds
        double maxChunkDurationInSeconds = maxChunkSizeInBytes / fileBitrate;

        // Convert the duration to minutes and round down to the nearest multiple of 10
        int chunkDurationInMinutes = (int)(maxChunkDurationInSeconds / 60);
        chunkDurationInMinutes = (chunkDurationInMinutes / 10) * 10;

        // Ensure the duration is at least 10 minutes
        return Math.Max(chunkDurationInMinutes, 10);
    }
    private async void DownloadAudioButton_Click(object sender, EventArgs e)
    {
        StatusLabel.Text = "Initializing";

        var videoInfo = await _processor.GetVideoInfo(UrlTextBox.Text);
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

            StatusLabel.Text = "Downloading";

            await _processor.DownloadAudioAsync(UrlTextBox.Text, saveDialog.FileName, downloadProgress);

            ProcessingProgressBar.Value = ProcessingProgressBar.Maximum;
            StatusLabel.Text = "Completed";
        }
    }
}