using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DockerTesting.Web.API
{
    internal static class HostingExtensions
    {
        public static Serilog.ILogger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
        }

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();

            builder.Services
                .AddRouting()
                .AddMvcCore()
                .AddApiExplorer()
                .AddDataAnnotations();

            builder.Services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = $"Docker Testing API",
                        Version = "v1"
                    });
                });

            return builder;
        }

        public static WebApplication ConfigurePipeline(this WebApplication application)
        {
            application.UseSerilogRequestLogging();

            application
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", $"Docker Testing API V1");
                    options.DocExpansion(DocExpansion.None);
                })
                .UseRouting();

            application.MapControllers();

            return application;
        }
    }
}
