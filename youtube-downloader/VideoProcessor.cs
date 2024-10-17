using System.Diagnostics;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader;

public class VideoProcessor
{
    private readonly YoutubeClient _youtube;
    public VideoProcessor()
    {
        _youtube = new YoutubeClient(); 
    }

    public async Task<Video> GetVideoInfo(string videoUrl)
    {
        return await _youtube.Videos.GetAsync(videoUrl);
    }

    public async Task<(string AudioFile, string VideoFile)> DownloadAsync(string videoUrl, IProgress<double> downloadProgress)
    {
        var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoUrl);

        var audioStreamInfo = streamManifest.Streams.OfType<AudioOnlyStreamInfo>()
            .Where(x => x.Container.Name == Container.Mp4.Name)
            .OrderByDescending(x => x.Bitrate)
            .FirstOrDefault();

        var videoStreamInfo = streamManifest.Streams.OfType<VideoOnlyStreamInfo>()
            .Where(x => x.Container.Name == Container.Mp4.Name)
            .OrderByDescending(s => s.VideoQuality)
            .FirstOrDefault();

        var audioFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetTempFileName()}.mp4");
        var videoFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetTempFileName()}.mp4");

        await _youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFilePath, downloadProgress);
        await _youtube.Videos.Streams.DownloadAsync(videoStreamInfo, videoFilePath, downloadProgress);

        return (audioFilePath, videoFilePath);

    }

public void MergeAsync(string audioFile, string videoFile, string outputFile)
{
    string ffmpegCommand = $"-y -i \"{videoFile}\" -i \"{audioFile}\" -c:v copy -c:a aac \"{outputFile}\"";

    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = ffmpegCommand,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            // Redirect standard input and close it to prevent FFmpeg from waiting for input
            RedirectStandardInput = true
        },
        EnableRaisingEvents = true
    };

    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
    process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

    process.Start();

    // Close standard input to prevent FFmpeg from waiting for input
    process.StandardInput.Close();

    process.BeginOutputReadLine();
    process.BeginErrorReadLine();

    process.WaitForExit();
}

}