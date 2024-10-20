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
    /// Creates a custom character for the LCD display at the specified location using the provided character map.
    /// </summary>
    public void CreateCustomCharacter(byte location, byte[] charmap) { }

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
