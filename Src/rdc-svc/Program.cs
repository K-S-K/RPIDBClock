using RPIDBClock.LCD;
using RPIDBClock.RTC;

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

    /// <summary>
    /// The main method of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    private static void Main(string[] args)
    {
        // Create a new instance of the DS3231 real-time clock.
        DS3231 rtc = new(DS3231_ADDRESS);

        // Create a new instance of the HD44780 LCD display.
        HD44780 lcd = new(HD44780_ADDRESS);
        lcd.Clear();

        // Display a greeting message.
        Console.WriteLine("Hello, Raspberry PI!");

        // Read the current time and temperature from the RTC module.
        for (int i = 0; i < 10; i++)
        {
            lcd.SetCursorPosition(0, 0);
            lcd.Write($"{rtc.ReadTime():yyyy.MM.dd HH:mm:ss}");
            lcd.SetCursorPosition(0, 1);
            lcd.Write($"Temperature: {rtc.ReadTemperature()}°C");

            Console.WriteLine($"Time: {rtc.ReadTime():yyyy.MM.dd HH:mm:ss}  Temperature: {rtc.ReadTemperature()}°C");
            Thread.Sleep(1000);
        }
    }
}
