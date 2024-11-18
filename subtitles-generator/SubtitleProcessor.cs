using System.Globalization;

namespace SubtitlesGenerator
{
    public static class SubtitleProcessor
    {
        public static void Merge(List<string> srtFiles, string outputFile)
        {
            // List to hold all subtitles
            List<Subtitle> allSubtitles = new List<Subtitle>();

            TimeSpan offset = TimeSpan.Zero;
            int indexCounter = 1;

            foreach (string srtFile in srtFiles)
            {
                Console.WriteLine($"Processing {srtFile}...");

                // Read and parse the subtitles from the current file
                List<Subtitle> subtitles = ParseSrtFile(srtFile);

                // Adjust the times and indices
                foreach (Subtitle subtitle in subtitles)
                {
                    subtitle.StartTime += offset;
                    subtitle.EndTime += offset;
                    subtitle.Index = indexCounter++;
                    allSubtitles.Add(subtitle);
                }

                // Update the offset based on the last subtitle's end time
                if (subtitles.Count > 0)
                {
                    offset += TimeSpan.FromMinutes(20); //[subtitles.Count - 1].EndTime;
                }
            }

            // Write all subtitles to the output file
            WriteSRTFile(outputFile, allSubtitles);

            Console.WriteLine("Merging complete!");
        }

        // Private helper method to parse an SRT file and return a list of Subtitle objects
        public static List<Subtitle> ParseSrtFile(string filePath)
        {
            List<Subtitle> subtitles = new List<Subtitle>();
            string[] lines = File.ReadAllLines(filePath);

            int index = 0;
            while (index < lines.Length)
            {
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(lines[index]))
                {
                    index++;
                    continue;
                }

                // Read index
                int subtitleIndex;
                if (!int.TryParse(lines[index++], out subtitleIndex))
                {
                    // Handle parsing error
                    throw new FormatException($"Invalid subtitle index at line {index} in file {filePath}.");
                }

                // Read time codes
                if (index >= lines.Length)
                    break;

                string timeLine = lines[index++];
                string[] timeParts = timeLine.Split(new string[] { " --> " }, StringSplitOptions.None);
                if (timeParts.Length != 2)
                {
                    // Handle parsing error
                    throw new FormatException($"Invalid time code format at subtitle {subtitleIndex} in file {filePath}.");
                }

                TimeSpan startTime = ParseTimeCode(timeParts[0]);
                TimeSpan endTime = ParseTimeCode(timeParts[1]);

                // Read subtitle text
                List<string> textLines = new List<string>();
                while (index < lines.Length && !string.IsNullOrWhiteSpace(lines[index]))
                {
                    textLines.Add(lines[index++]);
                }

                string text = string.Join(Environment.NewLine, textLines);

                // Create a new Subtitle object and add it to the list
                subtitles.Add(new Subtitle
                {
                    Index = subtitleIndex,
                    StartTime = startTime,
                    EndTime = endTime,
                    Text = text
                });
            }

            return subtitles;
        }

        // Private helper method to parse a time code string into a TimeSpan
        private static TimeSpan ParseTimeCode(string timeCode)
        {
            // Time code format: "hh:mm:ss,fff"
            if (TimeSpan.TryParseExact(timeCode, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture, out TimeSpan result))
            {
                return result;
            }
            else
            {
                // Handle parsing error
                throw new FormatException($"Invalid time code: {timeCode}");
            }
        }

        // Private helper method to write a list of subtitles to an SRT file
        private static void WriteSRTFile(string filePath, List<Subtitle> subtitles)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Subtitle subtitle in subtitles)
                {
                    writer.WriteLine(subtitle.Index);
                    writer.WriteLine($"{FormatTimeCode(subtitle.StartTime)} --> {FormatTimeCode(subtitle.EndTime)}");
                    writer.WriteLine(subtitle.Text);
                    writer.WriteLine();
                }
            }
        }

        // Private helper method to format a TimeSpan into an SRT time code string
        private static string FormatTimeCode(TimeSpan time)
        {
            // Ensure that the hours component is at least two digits
            int totalHours = (int)Math.Floor(time.TotalHours);
            int minutes = time.Minutes % 60;
            int seconds = time.Seconds % 60;
            int milliseconds = time.Milliseconds;
            return string.Format("{0:D2}:{1:D2}:{2:D2},{3:D3}", totalHours, minutes, seconds, milliseconds);
        }
    }
}
