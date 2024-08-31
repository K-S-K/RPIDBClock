using System.Diagnostics;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;

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
            return i2cSettings.LCDAddress > 0 ?
                new LCDService(i2cSettings.LCDAddress) : new LCDStub();
        });

        // Register RTC service
        builder.Services.AddSingleton<IRTCService>(provider =>
        {
            var i2cSettings = provider.GetRequiredService<IOptions<I2CSettings>>().Value;
            return i2cSettings.RTCAddress > 0 ?
                new RTCService(i2cSettings.RTCAddress) : new RTCStub();
        });

        // Register NTP and DBClock services
        builder.Services.AddSingleton<INTPService, NTPService>();
        builder.Services.AddSingleton<IDBClock, DBClock>();

        //  Build the application
        var app = builder.Build();

        // Get the host URL from configuration
        string hostUrl = builder.Configuration
                .GetValue<string>("HostSettings:Url") ?? "http://*:5000";

        // Configure the app to use the specified URL
        app.Urls.Add(hostUrl);

        // Open the port for incoming connections
        OpenPortForIncomingConnections(hostUrl);

        // Ressolve and start DBClock service.
        IDBClock clock = app.Services.GetRequiredService<IDBClock>();
        clock.Start();

        // Register routes
        app.MapGet("/", () => "Hello Raspberry PI!");

        // Run the application
        app.Run();
    }

    /// <summary>
    /// Opens the port for incoming connections.
    /// </summary>
    /// <param name="url">The URL to open the port for.</param>
    /// <remarks>
    /// This method uses the `ufw` command to open the port for incoming connections.
    /// </remarks>
    private static void OpenPortForIncomingConnections(string url)
    {
        // Extract the port number using a regular expression
        var match = Regex.Match(url, @":(\d+)");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int port))
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = $"ufw allow {port}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit();
        }
        else
        {
            Console.WriteLine($"Invalid URL format. Could not extract "
                + $"port number from the string \"{url}\".");
        }
    }
}
