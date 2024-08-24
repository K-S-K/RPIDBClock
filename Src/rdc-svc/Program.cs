using Microsoft.Extensions.Options;

using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET;

namespace RPIDBClock.Svc;

/// <summary>
/// Represents the entry point for the application.
/// </summary>
internal class Program
{
    /// <summary>
    /// The main method of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Bind I2CSettings section to I2CSettings class
        builder.Services.Configure<I2CSettings>(builder.Configuration.GetSection(nameof(I2CSettings)));

        // Register LCD service
        builder.Services.AddSingleton<ILCDService>(provider =>
        {
            var i2cSettings = provider.GetRequiredService<IOptions<I2CSettings>>().Value;
            return new LCDService(i2cSettings.LCDAddress);
        });

        // Register RTC service
        builder.Services.AddSingleton<IRTCService>(provider =>
        {
            var i2cSettings = provider.GetRequiredService<IOptions<I2CSettings>>().Value;
            return new RTCService(i2cSettings.RTCAddress);
        });

        // Register NTP and DBClock services
        builder.Services.AddSingleton<INTPService, NTPService>();
        builder.Services.AddSingleton<IDBClock, DBClock>();

        //  Build the application
        var app = builder.Build();

        // Ressolve and start DBClock service.
        IDBClock clock = app.Services.GetRequiredService<IDBClock>();
        clock.Start();

        // Register routes
        app.MapGet("/", () => "Hello Raspberry PI!");

        // Run the application
        app.Run();

        // Stop and dispose the clock
        clock.Pause();
        clock.Dispose();
    }
}
