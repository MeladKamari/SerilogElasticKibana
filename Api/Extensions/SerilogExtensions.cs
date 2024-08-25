using System.Diagnostics;
using System.Reflection;
using Elasticsearch.Net;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole.Themes;

namespace Api.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        #if DEBUG
        var selfLogFile = new StreamWriter("./logs/serilog-selflog.txt", append: true)
        {
            AutoFlush = true  
        };
       SelfLog.Enable(TextWriter.Synchronized(selfLogFile));
       SelfLog.Enable(Console.Error);
        #endif
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration
                // .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                // .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                #if DEBUG
                .MinimumLevel.Verbose()
                #endif
                #if (STAGE || RELEASE)
                .MinimumLevel.Information()
                #endif
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                        new Uri($"{context.Configuration.GetSection("ElasticSearch").GetValue<string>("URI")}"))
                    {
                        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}" +
                                      $"-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{TimeProvider.System.GetUtcNow():yyyy-MM}",
                        // ModifyConnectionSettings = x =>
                        //     x.BasicAuthentication(userName, password),
                        FailureCallback = (l, e) =>
                            Console.WriteLine("Unable submit event to ElasticSearch" + e.InnerException),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                           EmitEventFailureHandling.WriteToFailureSink |
                                           EmitEventFailureHandling.RaiseCallback,
                        AutoRegisterTemplate = true,
                        LevelSwitch = new LoggingLevelSwitch(LogEventLevel.Information),
                        // BufferBaseFilename = "./logs/serilog-buffer",
                        // BufferFileCountLimit = 10,   
                        // BufferFileSizeLimitBytes = 104_857_600,
                        // BufferFileRollingInterval = RollingInterval.Hour,
                        BatchPostingLimit = 50,
                        Period = TimeSpan.FromSeconds(2),
                        QueueSizeLimit = 100_000,
                        FailureSink = new FileSink("./logs/failures.txt", new JsonFormatter(), null),
                        ModifyConnectionSettings = conn =>
                        {
                            // conn.BasicAuthentication(userName, password);
                            conn.EnableHttpCompression(true);
#if DEBUG
                            conn.PrettyJson(true);
                            conn.EnableDebugMode(q => LogEventCallback(q));
#endif
                            return conn;
                        }
                    })
                // .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console(theme: SystemConsoleTheme.Colored,applyThemeToRedirectedOutput:true);

        });
        return hostBuilder;
    }
    private static void LogEventCallback(IApiCallDetails details) {
    }
}