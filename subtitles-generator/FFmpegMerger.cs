using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SubtitlesGenerator;

public class FFmpegMerger
{
    public static async Task Merge(string audioFile, string videoFile, string outputFile, IProgress<double> progress = null)
    {
        string ffmpegCommand = $"-y -i \"{videoFile}\" -i \"{audioFile}\" -c:v copy -c:a aac \"{outputFile}\"";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegCommand,
                RedirectStandardError = true,   // FFmpeg outputs progress info to stderr
                RedirectStandardInput = true,   // Close stdin to prevent FFmpeg from waiting for input
                UseShellExecute = false,
                CreateNoWindow = true,
            },
            EnableRaisingEvents = true
        };

        // Variables to store total duration
        double totalDurationInSeconds = 0;
        bool isDurationCaptured = false;

        // Regex patterns to extract duration and time
        var durationRegex = new Regex(@"Duration:\s(?<hh>\d{2}):(?<mm>\d{2}):(?<ss>\d{2}\.\d{2})");
        var timeRegex = new Regex(@"time=(?<hh>\d{2}):(?<mm>\d{2}):(?<ss>\d{2}\.\d{2})");

        process.Start();

        // Close standard input to prevent FFmpeg from waiting for input
        process.StandardInput.Close();

        // Read the standard error stream synchronously
        using (var reader = process.StandardError)
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                // Extract total duration only once
                if (!isDurationCaptured)
                {
                    var durationMatch = durationRegex.Match(line);
                    if (durationMatch.Success)
                    {
                        totalDurationInSeconds = TimeSpan.Parse($"{durationMatch.Groups["hh"].Value}:{durationMatch.Groups["mm"].Value}:{durationMatch.Groups["ss"].Value}").TotalSeconds;
                        isDurationCaptured = true;
                        Console.WriteLine($"Total Duration: {totalDurationInSeconds} seconds");
                    }
                }

                // Extract current time and calculate progress
                var timeMatch = timeRegex.Match(line);
                if (timeMatch.Success && totalDurationInSeconds > 0)
                {
                    var currentTime = TimeSpan.Parse($"{timeMatch.Groups["hh"].Value}:{timeMatch.Groups["mm"].Value}:{timeMatch.Groups["ss"].Value}");
                    double currentTimeInSeconds = currentTime.TotalSeconds;

                    double progressValue = (currentTimeInSeconds / totalDurationInSeconds) * 100;

                    // Clamp progress to 100%
                    progressValue = Math.Min(progressValue, 100);

                    // Report progress
                    progress?.Report(progressValue);
                    Console.WriteLine($"Progress: {progressValue:F2}%");
                }
            }
        }

        await process.WaitForExitAsync();

        // Check the exit code to determine success or failure
        if (process.ExitCode != 0)
        {
            throw new Exception($"FFmpeg exited with code {process.ExitCode}");
        }
    }
}