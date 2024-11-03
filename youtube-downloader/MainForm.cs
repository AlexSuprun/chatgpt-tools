using System.Diagnostics;
using FFMpegCore;
using SRTMerger;

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

    private const long MaxChunkSizeInBytes = 25 * 1024 * 1024;

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

    private async void TranscribeButton_Click(object sender, EventArgs e)
    {
        var audioFiles = Directory.GetFiles(@"C:\Users\alex.suprun\Desktop\Final\chunks", "*.mp4")
            .ToList();

        //await _audioTranscriber.TranscribeAll(audioFiles); 

        var subtitleFiles = Directory.GetFiles(@"C:\Users\alex.suprun\Desktop\Final\chunks", "*.srt")
            .ToList();

        SubtitleProcessor.Merge(subtitleFiles, @"C:\Users\alex.suprun\Desktop\Final\Wondershare Filmora 13 Complete Editing Tutorial for Beginners in 2024.srt");

        return;
    }

    private async Task SplitFile()
    {
        var audioFilePath = @"C:\Users\alex.suprun\Desktop\Final\audio-track.mp4";
        var mediaInfo = GetVideoInfo(audioFilePath);

        var chunkDurationInMinutes = CalculateChunkDuration(mediaInfo.fileSize, mediaInfo.duration);

        int totalChunks = (int)Math.Ceiling(mediaInfo.duration.TotalMinutes / chunkDurationInMinutes);

        for (int i = 0; i < totalChunks; i++)
        {
            // Calculate start and end time for each chunk
            TimeSpan from = TimeSpan.FromMinutes(i * chunkDurationInMinutes);
            TimeSpan to = from + TimeSpan.FromMinutes(chunkDurationInMinutes);

            // Ensure the last chunk does not exceed the total video duration
            if (to > mediaInfo.duration)
            {
                to = mediaInfo.duration;
            }

            // Generate the output path for each chunk
            string outputPath = Path.Combine(
                Path.GetDirectoryName(audioFilePath),
                $"{Path.GetFileNameWithoutExtension(audioFilePath)}_chunk_{i + 1}{Path.GetExtension(audioFilePath)}"
            );

            Console.WriteLine($"Splitting chunk {i + 1}: Start: {from}, End: {to}");

            // Split the file into chunks asynchronously, awaiting each chunk completion
            await FFMpeg.SubVideoAsync(audioFilePath, outputPath, from, to);
        }
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