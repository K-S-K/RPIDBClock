namespace RPIDBClock.LCD;

/// <summary>
/// Represents a service for interacting with an LCD display.
/// </summary>
/// <remarks>
/// This interface provides functionality to interact with an LCD display.
/// It supports various display functions such as clearing the display, 
/// writing text, creating custom characters, and setting the brightness level.
/// </remarks>
public class LCDService : ILCDService
{
    #region -> Fields
    private readonly HD44780 _lcd;
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="LCDService"/> class with the specified I2C device address.
    /// </summary>
    /// <param name="deviceAddress">The I2C device address of the LCD display.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the device address is invalid.</exception>
    /// <exception cref="PlatformNotSupportedException">Thrown when the platform is not supported.</exception>
    /// <exception cref="NotSupportedException">Thrown when the device is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the device access is denied.</exception>
    /// <exception cref="Exception">Thrown when an unknown error occurs.</exception>
    /// <remarks>
    /// The device address must be between 0x20 and 0x27.
    /// </remarks>
    public LCDService(byte deviceAddress)
    {
        _lcd = new HD44780(deviceAddress);
    }
    #endregion


    #region -> Methods
    /// <summary>
    /// Clears the LCD display.
    /// </summary>
    public void Clear()
    {
        _lcd.Clear();
    }

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
    public void Write(int row, int col, string text)
    {
        if (row < 0 || row > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "Row index must be between 0 and 3.");
        }
        if (col < 0 || col > 19)
        {
            throw new ArgumentOutOfRangeException(nameof(col), "Column index must be between 0 and 19.");
        }
        if (string.IsNullOrEmpty(text))
        {
            ArgumentNullException.ThrowIfNullOrEmpty(text, nameof(text));
        }

        _lcd.Write(row, col, text);
    }

    /// <summary>
    /// Creates a custom character for the LCD display at the specified location using the provided character map.
    /// </summary>
    /// <param name="location">The location where the custom character will be stored (0-7).</param>
    /// <param name="charMap">The character map representing the custom character.</param>
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
    public void CreateCustomCharacter(byte location, byte[] charMap)
    {
        if (location < 0 || location > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(location), "Location must be between 0 and 7.");
        }
        if (charMap == null || charMap.Length == 0)
        {
            ArgumentNullException.ThrowIfNull(charMap, nameof(charMap));
        }

        _lcd.CreateCustomCharacter(location, charMap);
    }

    /// <summary>
    /// Disposes of the resources (if any) used by the LCD display.
    /// </summary>
    /// <remarks>
    /// This method is called by the <see cref="Dispose()"/> method.
    /// </remarks>
    /// <seealso cref="Dispose()"/>
    /// <seealso cref="IDisposable"/>
    /// <seealso cref="ILCDService"/>
    /// <seealso cref="HD44780"/>
    /// <seealso cref="IDisposable"/>
    /// 
    public void Dispose()
    {
        _lcd.Dispose();
    }

    /// <summary>
    /// Sets the brightness level of the LCD display.
    /// </summary>
    /// <param name="level">The brightness level (0-100).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the brightness level is invalid.</exception>
    /// <exception cref="Exception">Thrown when an unknown error occurs.</exception>
    /// <remarks>
    /// The brightness level must be between 0 and 100.
    /// </remarks>
    public void SetBrightness(int level)
    {
        if (level < 0 || level > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Brightness level must be between 0 and 100.");
        }

        // Calculate the PWM value based on the brightness level.
        // byte pwm = (byte)(level * 255 / 100);
        // _lcd.SetBacklight(pwm);

        // The HD44780 LCD display does not support PWM for backlight control.
        // TODO: Implement a workaround for setting the brightness level.
    }

    /// <summary>
    /// Prepares the LCD display.
    /// </summary>
    /// <remarks>
    /// This method creates custom characters for the degree symbol (°), the clock symbol (🕒),
    /// the temperature symbol (🌡), and the humidity symbol (💧).
    /// </remarks>
    public void PrepareDisplay()
    {
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
        CreateCustomCharacter(0, degSymbol);

        // Create a custom character for the clock symbol (🕒).
        byte[] clockSymbol =
        [
            0b00000,
            0b01110,
            0b10101,
            0b10111,
            0b10001,
            0b01110,
            0b00000,
            0b00000
        ];
        CreateCustomCharacter(1, clockSymbol);

        // Create a custom character for the temperature symbol (🌡).
        byte[] tempSymbol =
        [
            0b00100,
            0b01010,
            0b01010,
            0b01110,
            0b01110,
            0b11111,
            0b11111,
            0b01110
        ];
        CreateCustomCharacter(2, tempSymbol);

        // Create a custom character for the humidity symbol (💧).
        byte[] humSymbol =
        [
            0b00000,
            0b00100,
            0b00100,
            0b01110,
            0b10101,
            0b10101,
            0b10001,
            0b01110
        ];
        CreateCustomCharacter(3, humSymbol);

        // Create a custom character for the earth symbol (🌍).
        byte[] earthSymbol =
        [
            0b00000,
            0b01110,
            0b11111,
            0b11111,
            0b01110,
            0b10101,
            0b11111,
            0b00000
        ];
        CreateCustomCharacter(4, earthSymbol);


        // Prepare the LCD display.
        Clear();
        Write(1, 18, "\x00");  // Display the custom character (°)
        Write(1, 19, "C");  // Display the temperature unit (C)

        // Display the custom character (🕒) for the clock symbol.
        Write(0, 0, "\x01");

        // Display the custom character (🌡) for the temperature symbol.
        Write(1, 0, "\x02");

        // Display the custom character (🌍) for the earth symbol
        Write(2, 0, "\x04");
    }
    #endregion
}
