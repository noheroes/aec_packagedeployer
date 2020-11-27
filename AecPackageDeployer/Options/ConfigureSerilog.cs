using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace AecPackageDeployer.Options
{
    public class ConfigureSerilog
    {
        public ConfigureSerilog(IConfiguration configuration)
        {
            var localPath = AppDomain.CurrentDomain.BaseDirectory;
            localPath = Path.Combine(localPath, "Log");

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath.ToLower());
            }

            string logFile = Path.Combine(localPath, "Log-.txt");
            const string template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                                    .ReadFrom.Configuration(configuration)
                                    .Enrich.FromLogContext()
                                    .WriteTo.File(logFile, outputTemplate: template, rollingInterval: RollingInterval.Day)
                                    .CreateLogger();
        }
    }
}
