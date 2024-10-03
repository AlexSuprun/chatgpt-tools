using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatToMarkdown;

public class ChatConverter
{
    public static (Conversation Conversation, List<MessageNode> Messages)  ExtractMessages(string json)
    {
        var messagesDict = new Dictionary<string, MessageNode>();

        var conversation = JsonConvert.DeserializeObject<Conversation>(json);
        // Build nodes
        foreach (var mappingItem in conversation.Mapping)
        {
            var nodeId = mappingItem.Key;
            var nodeData = mappingItem.Value;

            var messageNode = new MessageNode
            {
                Id = nodeData.Message?.Id,
                Author = nodeData.Message?.Author?.Role,
                Content = nodeData.Message?.Content?.Parts != null
                    ? string.Join("\n", nodeData.Message.Content.Parts)
                    : "",
                ParentId = nodeData.Parent,
                Children = new List<MessageNode>()
            };

            messagesDict[nodeId] = messageNode;
        }

        // Build tree
        foreach (var node in messagesDict.Values)
        {
            if (!string.IsNullOrEmpty(node.ParentId) && messagesDict.TryGetValue(node.ParentId, out var parent))
            {
                parent.Children.Add(node);
                node.IsRoot = false;
            }
            else
            {
                node.IsRoot = true;
            }
        }

        // Return root nodes
        var rootNodes = new List<MessageNode>();
        foreach (var node in messagesDict.Values)
        {
            if (node.IsRoot)
            {
                rootNodes.Add(node);
            }
        }

        return (conversation, rootNodes) ;
    }

    public static string ConvertToMarkdown(Conversation conversation, List<MessageNode> messages)
    {
        var markdown = new StringBuilder();

        markdown.AppendLine("---");
        var createdDateTime = conversation.CreateTimeInMs.ToDateTime();
        if(createdDateTime.HasValue)
        {
            markdown.AppendLine($"created: {createdDateTime.Value.ToString("yyyy-MM-dd")}");
        }

        markdown.AppendLine("is: \"[[ChatGPT Chat]]\"");
        
        markdown.AppendLine("---");
        markdown.AppendLine($"# {conversation.Title}");


        foreach (var message in messages)
        {
            AppendMessage(markdown, message);
        }

        return markdown.ToString();
    }

    static void AppendMessage(StringBuilder markdown, MessageNode message)
    {
        var author = message.Author != null ? message.Author.ToUpper() : "UNKNOWN";
        
        var codeBlockCount = Regex.Matches(message.Content, "```").Count;
        if (codeBlockCount % 2 != 0)
        {
            message.Content += "\n```";
        }

        if (author == "USER")
        {
            markdown.AppendLine($"\ud83d\ude4b **{author}** \ud83d\ude4b:");
            markdown.AppendLine();

            markdown.AppendLine($"> {message.Content.Replace("\n", "\n> ")}");
            markdown.AppendLine();
        }
        else if (author == "ASSISTANT")
        {
            markdown.AppendLine($"\ud83e\udd16 **{author}** \ud83e\udd16:");
            markdown.AppendLine();
            // Output assistant's message directly (supports markdown and code blocks)
            markdown.AppendLine(message.Content);
            markdown.AppendLine();
        }
        else
        {
            // For other roles (e.g., system), output normally
            markdown.AppendLine($"**{author}**:");
            markdown.AppendLine();
            markdown.AppendLine(message.Content);
            markdown.AppendLine();
        }

        // Process child messages recursively
        foreach (var child in message.Children)
        {
            AppendMessage(markdown, child);
        }
    }
}