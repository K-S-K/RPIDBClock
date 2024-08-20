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

    private static ILCDService? lcd;

    /// <summary>
    /// The main method of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    private static void Main(string[] args)
    {
        // Create a new instance of the DS3231 real-time clock.
        DS3231 rtc = new(DS3231_ADDRESS);

        // Create a new instance of the HD44780 LCD display.
        lcd = new LCDService(HD44780_ADDRESS);
        lcd.Clear();

        // Display a greeting message.
        Console.WriteLine("Hello, Raspberry PI!");

        // Get the current network time and set it on the RTC module.
        DateTime networkTime = NtpClient.GetNetworkTime();
        rtc.SetTime(networkTime);

        // Create a custom character for the degree symbol (°).
        byte[] degSymbol =
        [
            0b00110,
            0b01001,
            0b01001,
            0b00110,
            0b00000,
            0b00000,
            0b00000,
            0b00000
        ];
        lcd.CreateCustomCharacter(0, degSymbol);
        lcd.Write(1, 18, "\x00");  // Display the custom character (°)
        lcd.Write(1, 19, "C");  // Display the temperature unit (C)


        // Read the current time and temperature from the RTC module.
        for (int i = 0; i < 50; i++)
        {
            lcd.Write(0, 0, $"{rtc.ReadTime():yyyy.MM.dd HH:mm:ss}");
            lcd.Write(1, 0, $"Temperature: {rtc.ReadTemperature():F2}");

            Console.WriteLine($"Time: {rtc.ReadTime():yyyy.MM.dd HH:mm:ss}  Temperature: {rtc.ReadTemperature()}°C");
            Thread.Sleep(1000);
        }
    }
}
