using System.Device.I2c;

namespace RPIDBClock.LCD;

/// <summary>
/// Represents an LCD display with I2C communication.
/// </summary>
/// <remarks>
/// This class provides functionality to control an LCD display using I2C communication.
/// It supports various display functions such as clearing the display, returning to home, setting entry mode, controlling display, shifting cursor, and setting custom characters.
/// The class also allows controlling the display mode, backlight, and various display options such as cursor and blink.
/// </remarks>
public class HD44780 : IDisposable
{
    #region -> Constants
    private const byte LCD_CLR = 0x01;
    private const byte LCD_HOME = 0x02;
    private const byte LCD_ENTRY_MODE_SET = 0x04;
    private const byte LCD_DISPLAY_CONTROL = 0x08;
    private const byte LCD_CURSOR_SHIFT = 0x10;
    private const byte LCD_FUNCTION_SET = 0x20;
    private const byte LCD_CGRAM_ADDR = 0x40;
    private const byte LCD_DDRAM_ADDR = 0x80;

    private const byte LCD_BACKLIGHT = 0x08;
    private const byte ENABLE = 0x04;
    #endregion


    #region -> Fields
    private readonly I2cDevice _i2cDevice;
    private byte _displayControl;
    private byte _displayMode;
    private byte _backlight = LCD_BACKLIGHT;

    /// <summary>
    /// The synchronization object used to lock access to the I2C device.
    /// </summary>
    private object _sync = new();
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="HD44780"/> class with the specified I2C device.
    /// </summary>
    public HD44780(byte deviceAddress)
    {
        lock (_sync)
        {
            var settings = new I2cConnectionSettings(1, deviceAddress);
            _i2cDevice = I2cDevice.Create(settings);

            Initialize();
        }
    }
    #endregion


    #region -> Public Methods
    /// <summary>
    /// Clears the LCD display.
    /// </summary>
    public void Clear()
    {
        lock (_sync)
        {
            SendCommand(LCD_CLR);
            Thread.Sleep(2); // Clear command needs a longer delay
        }
    }

    /// <summary>
    /// Writes the specified text to the LCD display at the specified location.
    /// </summary>
    /// <param name="col">The column index of the cursor position.</param>
    /// <param name="row">The row index of the cursor position.</param>
    /// <param name="text">The text to be written.</param>
    public void Write(int row, int col, string text)
    {
        lock (_sync)
        {
            SetCursorPosition(col, row);
            Write(text);
        }
    }

    /// <summary>
    /// Creates a custom character for the LCD display at the specified location using the provided character map.
    /// </summary>
    /// <param name="location">The location where the custom character will be stored (0-7).</param>
    /// <param name="charmap">The character map representing the custom character.</param>
    public void CreateCustomCharacter(byte location, byte[] charmap)
    {
        lock (_sync)
        {
            location &= 0x7; // Only 8 locations available
            SendCommand((byte)(LCD_CGRAM_ADDR | (location << 3)));
            foreach (var line in charmap)
            {
                SendData(line);
            }
        }
    }

    /// <summary>
    /// Disposes the resources used by the LCD display.
    /// </summary>
    /// <remarks>
    /// This method releases the I2C device used by the LCD display.
    /// </remarks>
    public void Dispose()
    {
        lock (_sync)
        {
            _i2cDevice?.Dispose();
        }
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Initializes the LCD display.
    /// </summary>
    private void Initialize()
    {
        _displayControl = 0x04; // Display on, cursor off, blink off
        _displayMode = 0x02;    // Left to right

        SendCommand(0x33); // Initialize
        SendCommand(0x32); // Set to 4-bit mode
        SendCommand((byte)(LCD_FUNCTION_SET | 0x28)); // 2 lines, 5x8 matrix
        SendCommand((byte)(LCD_DISPLAY_CONTROL | _displayControl)); // Display on
        SendCommand((byte)(LCD_CLR)); // Clear display
        SendCommand((byte)(LCD_ENTRY_MODE_SET | _displayMode)); // Entry mode set
    }

    /// <summary>
    /// Sets the cursor position on the LCD display.
    /// </summary>
    /// <param name="col">The column index of the cursor position.</param>
    /// <param name="row">The row index of the cursor position.</param>
    private void SetCursorPosition(int col, int row)
    {
        int[] rowOffsets = [0x00, 0x40, 0x14, 0x54];
        SendCommand((byte)(LCD_DDRAM_ADDR | (col + rowOffsets[row])));
    }

    /// <summary>
    /// Writes the specified text to the LCD display.
    /// </summary>
    /// <param name="text">The text to be written.</param>
    private void Write(string text)
    {
        foreach (var c in text)
        {
            SendData((byte)c);
        }
    }

    /// <summary>
    /// Sends a command to the LCD display.
    /// </summary>
    /// <param name="command">The command to be sent.</param>
    private void SendCommand(byte command)
    {
        Send(command, 0x00);
    }

    /// <summary>
    /// Sends the specified data to the LCD display.
    /// </summary>
    /// <param name="data">The data to be sent.</param>
    private void SendData(byte data)
    {
        Send(data, 0x01);
    }

    /// <summary>
    /// Sends a byte value to the LCD display with the specified mode.
    /// </summary>
    /// <param name="value">The byte value to send.</param>
    /// <param name="mode">The mode of the LCD display.</param>
    private void Send(byte value, byte mode)
    {
        byte highNibble = (byte)(value & 0xF0);
        byte lowNibble = (byte)((value << 4) & 0xF0);

        WriteByte((byte)(highNibble | mode | _backlight));
        PulseEnable((byte)(highNibble | mode | _backlight));

        WriteByte((byte)(lowNibble | mode | _backlight));
        PulseEnable((byte)(lowNibble | mode | _backlight));
    }

    /// <summary>
    /// Writes a byte of data to the I2C device.
    /// </summary>
    /// <param name="data">The byte of data to write.</param>
    private void WriteByte(byte data)
    {
        _i2cDevice.WriteByte(data);
    }

    /// <summary>
    /// Pulses the enable signal to send data to the LCD.
    /// </summary>
    /// <param name="data">The data to be sent.</param>
    private void PulseEnable(byte data)
    {
        WriteByte((byte)(data | ENABLE));
        Thread.Sleep(1);
        WriteByte((byte)(data & ~ENABLE));
        Thread.Sleep(1);
    }
    #endregion
}
