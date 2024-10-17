using Newtonsoft.Json;

namespace ChatToMarkdown;

public class MappingNode
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("message")]
    public Message Message { get; set; }

    [JsonProperty("parent")]
    public string Parent { get; set; }

    [JsonProperty("children")]
    public List<string> Children { get; set; }
}