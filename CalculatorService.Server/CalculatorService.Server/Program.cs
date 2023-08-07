using CalculatorService.Server;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

public static class Program
{
    public static IConfiguration Configuration { get; private set; }
    public static async Task Main(string[] args)
    {
        //Log_log = new LogServices();
        string enviromentVar = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")) ?
                                "" :
                                "." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Build Configuration
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings" + enviromentVar + ".json", true)
            .AddCommandLine(args)
            .AddEnvironmentVariables()
            .Build();

        // Configure serilog
        Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(Configuration) 
             .CreateLogger();

        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Metodo: " + MethodBase.GetCurrentMethod() + "Message: "+ ex.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }    
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}