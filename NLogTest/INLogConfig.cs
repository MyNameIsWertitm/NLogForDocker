using Microsoft.AspNetCore.Hosting.Server;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System.Data;

namespace NLogTest
{
    public interface INLogConfig
    {
        public void AddNLogConfig();
    }

    public class NLogConfig : INLogConfig
    {
        public void AddNLogConfig()
        {
            try
            {
                var config = new LoggingConfiguration();

                // fileTarget ----{
                var fileJsonTraget = new FileTarget("FileJson")
                {
                    FileName = "${basedir}/logs/js.json",

                    KeepFileOpen = false,

                    CreateDirs = true,

                    Layout = new JsonLayout()
                    {
                        Attributes =
                        {
                            new JsonAttribute("message", "${event-properties:message}")
                        }
                    }
                };
                // ----}

                // databaseTarget ----{
                var databaseTarget = new DatabaseTarget()
                {
                    DBProvider = "Microsoft.Data.SqlClient",

                    Name = "Database",

                    ConnectionString = "Server=localhost;Database=testik;Trusted_Connection=True;TrustServerCertificate=True;",

                    CommandText = "INSERT INTO [dbo].[TEST_LOG_TABLE] ( [LOG_MESSAGE] )" +
                                   "VALUES (@LOG_MESSAGE);"
                };

                databaseTarget.Parameters.Add(new DatabaseParameterInfo { Name = "@LOG_MESSAGE", Layout = "${event-properties:message}" });
                // ----}

                config.AddTarget(fileJsonTraget);
                config.AddTarget(databaseTarget);

                config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, databaseTarget);
                config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileJsonTraget);

                LogManager.Configuration = config;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }
}

