using Newtonsoft.Json;

namespace ChatToMarkdown;

public class Conversation
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("create_time")]
    public double? CreateTimeInMs { get; set; }

    [JsonProperty("update_time")]
    public double? UpdateTimeInMs { get; set; }

    [JsonProperty("mapping")]
    public Dictionary<string, MappingNode> Mapping { get; set; }
}