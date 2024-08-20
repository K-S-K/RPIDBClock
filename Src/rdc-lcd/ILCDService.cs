namespace RPIDBClock.LCD;

/// <summary>
/// Represents a service for interacting with an LCD display.
/// </summary>
/// <remarks>
/// This interface provides functionality to interact with an LCD display.
/// It supports various display functions such as clearing the display, 
/// writing text, creating custom characters, and setting the brightness level.
/// </remarks>
public interface ILCDService
{
    #region -> Methods
    /// <summary>
    /// Clears the LCD display.
    /// </summary>
    void Clear();

    /// <summary>
    /// Writes the specified text to the LCD display at the specified location.
    /// </summary>
    /// <param name="row">The row index of the cursor position.</param>
    /// <param name="col">The column index of the cursor position.</param>
    /// <param name="text">The text to be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the row index is invalid.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the column index is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the text is null or empty.</exception>
    /// <exception cref="Exception">Thrown when an unknown error occurs.</exception>
    /// <remarks>
    /// The row index must be between 0 and 3.
    /// The column index must be between 0 and 19.
    /// The text must not be null or empty.
    /// </remarks>
    void Write(int row, int col, string text);

    /// <summary>
    /// Creates a custom character for the LCD display at the specified location using the provided character map.
    /// </summary>
    /// <param name="location">The location where the custom character will be stored (0-7).</param>
    /// <param name="charmap">The character map representing the custom character.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the location is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the character map is null or empty.</exception>
    /// <exception cref="Exception">Thrown when an unknown error occurs.</exception>
    /// <remarks>
    /// The location must be between 0 and 7.
    /// The character map must not be null or empty.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Create a custom character for the degree symbol (°)
    /// byte[] degSymbol =
    /// {
    ///    0b00110,
    ///    0b01001,
    ///    0b01001,
    ///    0b00110,
    ///    0b00000,
    ///    0b00000,
    ///    0b00000,
    ///    0b00000
    ///    };
    ///    lcd.CreateCustomCharacter(0, degSymbol);
    ///    lcd.Write(1, 18, "\x00");  // Display the custom character (°)
    ///    lcd.Write(1, 19, "C");  // Display the temperature unit (C)
    /// }
    /// </code>
    /// </example>
    void CreateCustomCharacter(byte location, byte[] charmap);

    /// <summary>
    /// Sets the brightness level of the LCD display.
    /// </summary>
    /// <param name="level">The brightness level (0-100).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the brightness level is invalid.</exception>
    /// <exception cref="Exception">Thrown when an unknown error occurs.</exception>
    /// <remarks>
    /// The brightness level must be between 0 and 100.
    /// </remarks>
    void SetBrightness(int level);
    #endregion
}