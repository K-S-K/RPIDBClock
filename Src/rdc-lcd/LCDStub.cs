namespace RPIDBClock.LCD;

/// <summary>
/// Represents a service for replacing real interacting with an LCD display.
/// </summary>
public class LCDStub : ILCDService
{
    /// <summary>
    /// Clears the LCD display.
    /// </summary>
    public void Clear()
    {
        Console.WriteLine("LCDStub: Clear()");
    }

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

        Console.WriteLine($"LCDStub: Write({row}, {col}, \"{text}\")");
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
    {
        Console.WriteLine($"LCDStub: SetBrightness({level})");
    }
}
