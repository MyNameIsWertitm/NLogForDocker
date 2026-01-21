using NLog;

namespace NLogTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<INLogConfig, NLogConfig>();

            var app = builder.Build();

            app.MapGet("/nlogTest", (INLogConfig _nlog) =>
            {
                try
                {
                    _nlog.AddNLogConfig();
                    Logger logger = LogManager.GetCurrentClassLogger();
                    var logEvent = new LogEventInfo();
                    logEvent.Properties["message"] = "test log; " + DateTime.Now.ToString();
                    logEvent.Level = NLog.LogLevel.Info;
                    logger.Log(logEvent);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            });

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
