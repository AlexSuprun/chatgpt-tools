using Microsoft.Extensions.Configuration;

namespace SubtitlesGenerator;

static class Program
{
    
    public static IConfiguration Configuration { get; private set; }
    public static OpenAiConfiguration OpenAiConfig { get; private set; }
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        Configuration = builder.Build();
        var openAiConfiguration = new OpenAiConfiguration();
        Configuration.GetSection("OpenAI").Bind(openAiConfiguration);

        // Store the configuration for later use
        OpenAiConfig = openAiConfiguration;
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}