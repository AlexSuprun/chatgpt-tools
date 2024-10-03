using Newtonsoft.Json;

namespace ChatToMarkdown;

public class Content
{
    [JsonProperty("content_type")]
    public string ContentType { get; set; }

    [JsonProperty("parts")]
    public List<string> Parts { get; set; }
}