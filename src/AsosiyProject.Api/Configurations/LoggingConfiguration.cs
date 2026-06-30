using Serilog;
using Serilog.Sinks.PostgreSQL;

namespace AsosiyProject.Api.Configurations;

public static class LoggingConfiguration
{
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "Timestamp", new TimestampColumnWriter() },
            { "Level", new LevelColumnWriter() },
            { "Message", new RenderedMessageColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "Properties", new PropertiesColumnWriter() }
        };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] (Thread:{ThreadId}) {Message:lj} {Properties}{NewLine}{Exception}")
            .WriteTo.PostgreSQL(
                connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                tableName: "Logs",
                columnOptions: columnWriters,
                needAutoCreateTable: false
            )
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}