using System.Text;
using System.Text.RegularExpressions;

namespace TimeOs2Srt;

public static class TranscriptConverter
{
    public static string ConvertToSrt(string transcript)
    {
        // Split the transcript into individual lines
        var lines = transcript.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // List to hold parsed transcript entries
        var entries = new List<TranscriptEntry>();

        // Regular expression to parse each line of the transcript
        var lineRegex = new Regex(@"^(Speaker \d+): \((\d{1,2}:\d{2})\) : (.*)$");

        // Variables to handle hour offset due to missing hour in timestamps
        double previousTotalSeconds = 0;
        int hourOffset = 0;

        foreach (var line in lines)
        {
            var match = lineRegex.Match(line.Trim());
            if (match.Success)
            {
                string speaker = match.Groups[1].Value;
                string timeString = match.Groups[2].Value;
                string text = match.Groups[3].Value;

                // Parse minutes and seconds
                var timeParts = timeString.Split(':');
                int minutes = int.Parse(timeParts[0]);
                int seconds = int.Parse(timeParts[1]);

                // Calculate total seconds with the current hour offset
                double totalSeconds = (hourOffset * 3600) + (minutes * 60) + seconds;

                // If totalSeconds is less than previousTotalSeconds, increment hourOffset
                if (totalSeconds < previousTotalSeconds)
                {
                    hourOffset++;
                    totalSeconds = (hourOffset * 3600) + (minutes * 60) + seconds;
                }

                previousTotalSeconds = totalSeconds;

                TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

                entries.Add(new TranscriptEntry
                {
                    Speaker = speaker,
                    StartTime = time,
                    Text = text
                });
            }
            else
            {
                throw new FormatException($"Invalid line format: {line}");
            }
        }

        // Build the SRT content
        var srtBuilder = new StringBuilder();
        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            TimeSpan endTime;

            // Determine the end time for the current subtitle
            if (i + 1 < entries.Count)
            {
                var nextEntry = entries[i + 1];
                endTime = nextEntry.StartTime;

                // Ensure end time is after start time
                if (endTime <= entry.StartTime)
                {
                    endTime = entry.StartTime + TimeSpan.FromSeconds(2);
                }
            }
            else
            {
                // For the last entry, add a default duration
                endTime = entry.StartTime + TimeSpan.FromSeconds(2);
            }

            srtBuilder.AppendLine((i + 1).ToString());
            srtBuilder.AppendLine($"{FormatTime(entry.StartTime)} --> {FormatTime(endTime)}");
            srtBuilder.AppendLine($"{entry.Text}");
            srtBuilder.AppendLine();
        }

        return srtBuilder.ToString();
    }

    private static string FormatTime(TimeSpan time)
    {
        int totalHours = (int)time.TotalHours;
        int minutes = time.Minutes;
        int seconds = time.Seconds;
        int milliseconds = time.Milliseconds;

        return string.Format("{0:D2}:{1:D2}:{2:D2},{3:D3}",
            totalHours,
            minutes,
            seconds,
            milliseconds);
    }
}