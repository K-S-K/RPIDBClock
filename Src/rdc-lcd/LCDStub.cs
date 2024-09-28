using System.Text;

namespace RPIDBClock.LCD;

/// <summary>
/// Represents a service for replacing real interacting with an LCD display.
/// </summary>
public class LCDStub : ILCDService
{
    #region -> Test Support
    /// <summary>
    /// Log data collector.
    /// </summary>
    /// <remarks>
    /// This field collects the log data during the interaction with the LCD display.
    /// </remarks>
    private readonly StringBuilder _log = new();

    /// <summary>
    /// The log data collected during the interaction with the LCD display.
    /// </summary>
    /// <remarks>
    /// This property provides the log data collected during the interaction with the LCD display.
    /// </remarks>
    public string Log => _log.ToString();

    /// <summary>
    /// Writes the specified message to the console and the log.
    /// </summary>
    private void WriteLine(string message)
    {
        _log.AppendLine(message);
        Console.WriteLine(message);
    }

    /// <summary>
    /// Adds a separator to the log.
    /// </summary>
    public void AddLogSeparator()
        => WriteLine("--------------------------------------------------");

    /// <summary>
    /// Adds a separator with the specified message to the log.
    /// </summary>
    public void AddLogSeparator(string message)
        => WriteLine($"--------------------------------------- {message}");
    #endregion


    #region -> ILCDService Implementation
    /// <summary>
    /// Clears the LCD display.
    /// </summary>
    public void Clear() => WriteLine("LCDStub: Clear()");

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

        WriteLine($"LCDStub: Write({row}, {col}, \"{text}\")");
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
        => WriteLine($"LCDStub: SetBrightness({level})");

    /// <summary>
    /// Prepares the LCD display.
    /// </summary>
    public void PrepareDisplay()
        => WriteLine("LCDStub: PrepareDisplay()");
    #endregion
}
