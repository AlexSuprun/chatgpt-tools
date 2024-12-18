using Microsoft.Extensions.Configuration;

namespace YoutubeDownloader;

static class Program
{
    
    public static IConfiguration Configuration { get; private set; }
    
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
        
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}