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
    /// The I2C address of the DS3231 RTC module.
    /// </summary>
    private const byte DS3231_ADDRESS = 0x68;

    /// <summary>
    /// The I2C address of the HD44780 LCD module.
    /// </summary>
    private const byte HD44780_ADDRESS = 0x27;

    private static INTPService? ntp;

    private static ILCDService? lcd;

    private static IRTCService? rtc;

    /// <summary>
    /// The main method of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    private static void Main(string[] args)
    {
        // Create a new instance of the NTP client.
        ntp = new NTPService();

        // Create a new instance of the DS3231 real-time clock.
        rtc = new RTCService(DS3231_ADDRESS);

        // Create a new instance of the HD44780 LCD display.
        lcd = new LCDService(HD44780_ADDRESS);

        // Create a new instance of the clock.
        DBClock clock = new(ntp, lcd, rtc);
        clock.Start();

        // Wait for 20 seconds.
        for (int i = 0; i < 20; i++)
        {
            Console.WriteLine($"[{i}]");
            Thread.Sleep(1000);
        }

        clock.Pause();
        clock.Dispose();
    }
}
