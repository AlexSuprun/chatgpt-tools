using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatToMarkdown;

public class Content
{
    [JsonProperty("content_type")]
    public string ContentType { get; set; }

    [JsonProperty("parts")]
    public List<JToken> Parts { get; set; }
}