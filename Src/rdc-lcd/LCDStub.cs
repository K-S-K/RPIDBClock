using RPIDBClock.Log;

namespace RPIDBClock.LCD;

/// <summary>
/// Represents a stub LCD service for replacing real one.
/// </summary>
public class LCDStub(ISimpleLogger logger) : ILCDService
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ISimpleLogger _logger = logger;


    /// <summary>
    /// Clears the LCD display.
    /// </summary>
    public void Clear() => _logger.AddEvent(LogEventClass.LCD, LogEventMethod.Clear);

    /// <summary>
    /// Writes the specified text to the LCD display at the specified location.
    /// </summary>
    /// <param name="row">The row index of the cursor position.</param>
    /// <param name="col">The column index of the cursor position.</param>
    /// <param name="text">The text to be written.</param>
    public void Write(int row, int col, string text)
    {
        if (text.Length == 1)
        {
            if (text.ToCharArray()[0] < 8)
            {
                text = "#";
            }
        }

        _logger.AddEvent(LogEventClass.LCD, LogEventMethod.Write, $"Row: {row}, Col: {col}, Text: {text}");
    }

    /// <summary>
    /// Writes the current date to the LCD display.
    /// </summary>
    /// <param name="date">The date to be written.</param>
    /// 
    public void WriteDateTime(DateTime time, int row = 0)
    {
        // Format the date and time as a string.
        string dateTimeString = $"{time:yyyy.MM.dd HH:mm:ss}";

        // Write the date and time to the LCD display.
        Write(row, 1, dateTimeString);
    }

    /// <summary>
    /// Writes the current temperature to the LCD display.
    /// </summary>
    /// <param name="temp">The temperature to be written.</param>
    public void WriteTemperature(double temp)
    {
        // Write the temperature to the LCD display.
        Write(0, 1, $"Temperature: {temp:F1}°C");
    }

    /// <summary>
    /// Writes the current time zone to the LCD display.
    /// </summary>
    public void WriteTimeZone()
    {
        // Get the local time zone information.
        TimeZoneInfo.ClearCachedData();
        var tz = TimeZoneInfo.Local;

        // Write the time zone to the LCD display.
        Write(0, 1, $"Time Zone: {tz.Id}");
    }

    /// <summary>
    /// Writes a log event message to the LCD display.
    /// </summary>
    public void WriteLogEvent(string msg)
    {
        Write(3, 0, msg);
    }

    /// <summary>
    /// Creates a custom character for the LCD display at the specified location using the provided character map.
    /// </summary>
    /// <param name="location">The location where the custom character will be stored (0-7).</param>
    /// <param name="charMap">The character map representing the custom character.</param>
    public void CreateCustomCharacter(byte location, byte[] charMap) { }

    /// <summary>
    /// Sets the backlight brightness level of the LCD display.
    /// </summary>
    /// <param name="level">The brightness level (0-255).</param>
    public void SetBrightness(int level)
        => _logger.AddEvent(LogEventClass.LCD, LogEventMethod.SetBrightness, $"Level: {level}");

    /// <summary>
    /// Prepares the LCD display.
    /// </summary>
    public void PrepareDisplay()
        => _logger.AddEvent(LogEventClass.LCD, LogEventMethod.PrepareDisplay);
}
