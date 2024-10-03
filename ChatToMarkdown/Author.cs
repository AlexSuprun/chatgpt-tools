using Newtonsoft.Json;

namespace ChatToMarkdown;

public class Author
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("metadata")]
    public object Metadata { get; set; }
}
