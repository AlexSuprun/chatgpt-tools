namespace YoutubeDownloader;

public partial class MainForm : Form
{
    private readonly VideoProcessor _processor = new VideoProcessor();
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
            
            StatusLabel.Text = "Merging";
            await FFmpegMerger.Merge(videoParts.AudioFile, videoParts.VideoFile, saveDialog.FileName, mergeProgress);
            File.Delete(videoParts.AudioFile);
            File.Delete(videoParts.VideoFile);
            
            StatusLabel.Text = "Completed"; 
        }
    }

    // async Task DownloadYouTubeVideo(string videoUrl, string outputDirectory)
    // {
    //     var processor = new VideoProcessor();
    //
    //     var downloadProgress = new Progress<double>(value =>
    //     {
    //         StatusLabel.Text = "Downloading: " + value.ToString();
    //         ProcessingProgressBar.Value = (int)(value * 100);
    //     });
    //
    //     var processProgress = new Progress<double>(value =>
    //     {
    //         StatusLabel.Text = "Processing: " + value.ToString();
    //         ProcessingProgressBar.Value = (int)(value * 100);
    //     });
    //
    //
    //
    //     var videoInfo = await processor.GetVideoInfo(videoUrl);
    //     var sanitizedTitle = string.Join("_", videoInfo.Title.Split(Path.GetInvalidFileNameChars()));
    //
    //     var result =  await processor.DownloadAsync(videoUrl, @"C:\Users\alex.suprun\Desktop\Youtube downloaded", downloadProgress, processProgress);
    //     await processor.MergeAsync(result.AudioFile, result.VideoFile, @"C:\Users\alex.suprun\Desktop\Youtube downloaded\mergedVideo.mp4");
    //
    //     File.Delete(result.AudioFile);
    //     File.Delete(result.VideoFile); 
    // }
}