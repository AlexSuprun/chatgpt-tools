using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace SubtitlesGenerator;

public class TranslationService
{
    private  static Lazy<Kernel> _kernelLazy = new(CreateKernel);
    public static OpenAiConfiguration GetOpenAiConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var openAiConfiguration = new OpenAiConfiguration();
        configuration.GetSection("OpenAI").Bind(openAiConfiguration);

        return openAiConfiguration; 
    }
    
    public static Kernel CreateKernel()
    {
        var openAiConfiguration = GetOpenAiConfiguration();

        var kernelBuilder = Kernel.CreateBuilder(); 
        kernelBuilder.Services.AddOpenAIChatCompletion(openAiConfiguration.Model, openAiConfiguration.ApiKey);

        return kernelBuilder.Build(); 
    }
    public static async Task<List<Subtitle>> TranslateBatchAsync(List<Subtitle> batch)
    {
        var textToTranslate = string.Join("\n\n", batch.ConvertAll(s => s.Text));

        var prompt = $"Translate the following text from English to Hebrew, preserving the formatting and line breaks. Return only Hebrew text:\n\n{textToTranslate}";

        var result = await _kernelLazy.Value.InvokePromptAsync(prompt);
        
        var translatedTexts = result.ToString().Split(["\n\n"], StringSplitOptions.None);
        for (int i = 0; i < translatedTexts.Length; i++)
        {
            batch[i].Text = translatedTexts[i];
        }
        
        return batch;
    }
}