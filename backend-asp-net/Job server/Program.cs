using Hangfire;
using Job_server.Jobs;
using Job_server.Options;

namespace Job_server;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseInMemoryStorage());

        builder.Services.AddHangfireServer();
        builder.Services.AddSingleton(typeof(DeductRentJob));

        builder.Configuration.AddJsonFile("Configs/connection.json");

        builder.Services.Configure<ConnectionConfiguration>(
            builder.Configuration.GetSection("ConnectionConfiguration"));

        var app = builder.Build();
        app.UseHangfireDashboard("/hf");

        InitJobs(builder);

        app.Run();
    }

    private static void InitJobs(WebApplicationBuilder builder)
    {
        var provider = builder.Services.BuildServiceProvider();

        var job = provider.GetService(typeof(DeductRentJob)) as DeductRentJob;

        RecurringJob
            .AddOrUpdate("balance_job", () => job!.DeductRent(), () => "0 21 * * *", new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Ekaterinburg Standard Time")
            });
    }
}