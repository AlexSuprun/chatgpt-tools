namespace ChatToMarkdown;

public class MessageNode
{
    public string Id { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public string ParentId { get; set; }
    public bool IsRoot { get; set; }
    public List<MessageNode> Children { get; set; } = new();
}