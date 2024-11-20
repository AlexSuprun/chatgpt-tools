using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

var path =
    @"C:\Users\me\OneDrive\Desktop\Advanced Voice Model\33,000 Advanced Voice Prompts 14447048fd1f819abc65def6789dbab4.csv";

string markdownFilePath =
    @"C:\Users\me\OneDrive\מסמכים\Vaults\PKM\+\output.md"; // Path to the output markdown file

// Read CSV records
using (var reader = new StreamReader(path))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    var records = csv.GetRecords<Record>()
        .ToList();

    using (var writer = new StreamWriter(markdownFilePath))
    {
        foreach (var category in records.GroupBy(x => x.Category).OrderBy(x => x.Key))
        {
            writer.WriteLine($"# {category.Key}");

            writer.WriteLine();
            foreach (var record in category)
            {
                writer.WriteLine($"## {record.Title}");
                writer.WriteLine($"{record.Description}");
                writer.WriteLine($"### Voice Prompts");

                try
                {
                    var prompts = ParseVoicePrompts(record.VoicePrompts);
                    foreach (var prompt in prompts)
                    {
                        writer.WriteLine($"#### {prompt.Title}");
                        writer.WriteLine($"{prompt.Prompt}");
                    }
                }
                catch
                {
                    writer.WriteLine($"{record.VoicePrompts}");
                }
            }
        }
    }
}

static List<VoicePrompt> ParseVoicePrompts(string input)
{
    var voicePrompts = new List<VoicePrompt>();
    
    var lines = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

    for (var i = 0; i < lines.Length; i++)
    {
        var voicePromtParts = lines[i].Split("\n");

        var voicePrompt = new VoicePrompt()
        {
            Title = voicePromtParts[0],
            Prompt = voicePromtParts[1]
        };

        var nextLine = i + 1;
        
        if (nextLine < lines.Length && !lines[nextLine].Contains("\n"))
        {
            voicePrompt.Prompt+=$"\n{lines[nextLine]}";
            i++; 
        }
        
        voicePrompts.Add(voicePrompt);
    }

    return voicePrompts;
    
    if (lines.Length == 0)
        return [];

    // The first line is the title
    var title = lines[0].Trim();

    // The rest is the prompt
    var promptLines = new List<string>();

    // Collect the prompt lines (could be one or more lines)
    for (int i = 1; i < lines.Length; i++)
    {
        promptLines.Add(lines[i].Trim());
    }

    var prompt = string.Join("\n", promptLines);

    // Add the voice prompt to the collection
    voicePrompts.Add(new VoicePrompt
    {
        Title = title,
        Prompt = prompt
    });

    return voicePrompts;
}

Console.WriteLine("Markdown file has been generated successfully.");


public class Record
{
    public string Favorites { get; set; }
    public string Title { get; set; }
    [Name("Voice Prompts")] public string VoicePrompts { get; set; }
    public string Category { get; set; }
    public string Field { get; set; }
    public string Description { get; set; }
}

public class VoicePrompt
{
    public string Title { get; set; }
    public string Prompt { get; set; }
}