using System.Net.Http.Headers;
using System.Text;

namespace SubtitlesGenerator
{
    public class AudioTranscriber
    {
        private readonly string _apiKey;

        public AudioTranscriber(string apiKey)
        {
            _apiKey = apiKey;
        }

        
        public async Task<string> TranscribeAll(IList<string> chunks)
        {
            var allSubtitles = new List<string>();

            // Transcribe each chunk and collect the results
            for (int i = 0; i < chunks.Count; i++)
            {
                string chunkFilePath = chunks[i];
                var srtContent = await TranscribeToSrtAsync(chunkFilePath);
                if (!string.IsNullOrEmpty(srtContent))
                {
                    allSubtitles.Add(srtContent);
                }

                var srtFolderName = Path.GetDirectoryName(chunkFilePath); 
                var srtChunkFileName = Path.GetFileNameWithoutExtension(chunkFilePath)+".srt";
                var chunkSrtFilePath = Path.Combine(srtFolderName, srtChunkFileName);

                await using (var writer = new StreamWriter(chunkSrtFilePath))
                {
                    await writer.WriteAsync(srtContent); 
                }
                //File.Delete(chunkFilePath); // Clean up chunk file
            }

            return string.Empty;    
            // Combine all subtitles into a single string
            return MergeSrtFiles(allSubtitles);
        }
        

        // Method to transcribe an individual file to SRT format
        public async Task<string> TranscribeToSrtAsync(string audioFilePath)
        {
            if (!File.Exists(audioFilePath))
                throw new FileNotFoundException("Audio file not found.", audioFilePath);

            var url = "https://api.openai.com/v1/audio/transcriptions";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                httpClient.Timeout = TimeSpan.FromMinutes(5); 
                
                using (var content = new MultipartFormDataContent())
                {
                    try
                    {
                        var audioBytes = await File.ReadAllBytesAsync(audioFilePath);
                        var audioContent = new ByteArrayContent(audioBytes);

                        string contentType = GetContentType(audioFilePath);
                        audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                        content.Add(audioContent, "file", Path.GetFileName(audioFilePath));
                        content.Add(new StringContent("whisper-1"), "model");
                        content.Add(new StringContent("srt"), "response_format");

                        var response = await httpClient.PostAsync(url, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return responseContent; // Return .srt content
                        }

                        Console.WriteLine("Error: " + responseContent);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }

        // Method to split audio file into chunks
        private List<string> SplitAudioFile(string inputFilePath, int chunkDurationInSeconds)
        {
            var outputFiles = new List<string>();

            

            return outputFiles;
        }


        // Utility method to get content type based on file extension
        private string GetContentType(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
            return fileExtension switch
            {
                ".mp3" => "audio/mpeg",
                ".mp4" => "audio/mp4",
                ".mpeg" => "audio/mpeg",
                ".mpga" => "audio/mpeg",
                ".m4a" => "audio/m4a",
                ".wav" => "audio/wav",
                ".webm" => "audio/webm",
                _ => throw new NotSupportedException("Unsupported audio format")
            };
        }

        // Merge multiple SRT strings into one
        private string MergeSrtFiles(List<string> srtFilesContent)
        {
            var mergedSrt = new StringBuilder();
            int subtitleIndex = 1;

            foreach (var srtContent in srtFilesContent)
            {
                var lines = srtContent.Split('\n');
                foreach (var line in lines)
                {
                    // Update subtitle numbering
                    if (int.TryParse(line.Trim(), out _))
                    {
                        mergedSrt.AppendLine(subtitleIndex.ToString());
                        subtitleIndex++;
                    }
                    else
                    {
                        mergedSrt.AppendLine(line);
                    }
                }
            }

            return mergedSrt.ToString();
        }
    }
}