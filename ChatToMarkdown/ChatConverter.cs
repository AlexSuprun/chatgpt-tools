using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatToMarkdown;

public class ChatConverter
{
    public static List<MessageNode> ExtractMessages(string json)
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
            if (!string.IsNullOrEmpty(node.ParentId) && messagesDict.ContainsKey(node.ParentId))
            {
                var parent = messagesDict[node.ParentId];
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

        return rootNodes;
    }

    public static string ConvertToMarkdown(List<MessageNode> messages)
    {
        var markdown = new StringBuilder();

        foreach (var message in messages)
        {
            AppendMessage(markdown, message);
        }

        return markdown.ToString();
    }

    static void AppendMessage(StringBuilder markdown, MessageNode message)
    {
        markdown.AppendLine("---");
        markdown.AppendLine("---");
        markdown.AppendLine("---");
        markdown.AppendLine($"**{message.Author?.ToUpper()}**:");
        markdown.AppendLine();
        markdown.AppendLine($"{message.Content}");
        markdown.AppendLine();

        foreach (var child in message.Children)
        {
            AppendMessage(markdown, child);
        }
    }
}