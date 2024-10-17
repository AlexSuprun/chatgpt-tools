using Newtonsoft.Json;

namespace ChatToMarkdown;

public class Message
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("author")]
    public Author Author { get; set; }

    [JsonProperty("create_time")]
    public double? CreateTime { get; set; }

    [JsonProperty("content")]
    public Content Content { get; set; }

    // Other properties can be added as needed
}