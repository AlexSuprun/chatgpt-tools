using System.Net.Http.Headers;

namespace YoutubeDownloader;

public class AudioTranscriber
{
    private readonly string _apiKey;

    public AudioTranscriber(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> TranscribeToSrtAsync(string audioFilePath)
    {
        if (!File.Exists(audioFilePath))
            throw new FileNotFoundException("Audio file not found.", audioFilePath);

        var url = "https://api.openai.com/v1/audio/transcriptions";

        using (var httpClient = new HttpClient())
        {
            // Set the authorization header with the API key
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Create the multipart form data content
            using (var content = new MultipartFormDataContent())
            {
                // Read the audio file bytes
                var audioBytes = await File.ReadAllBytesAsync(audioFilePath);
                var audioContent = new ByteArrayContent(audioBytes);

                // Set the content type based on the file extension
                var fileExtension = Path.GetExtension(audioFilePath).ToLowerInvariant();
                string contentType = fileExtension switch
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
                audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                // Add the audio file content
                content.Add(audioContent, "file", Path.GetFileName(audioFilePath));

                // Add other parameters
                content.Add(new StringContent("whisper-1"), "model");
                content.Add(new StringContent("srt"), "response_format");

                // Send the POST request
                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return responseContent; // This is your .srt content
                }

                Console.WriteLine("Error: " + responseContent);
                return null;
            }
        }
    }
}