using System.Device.I2c;

namespace RPIDBClock.RTC;

/// <summary>
///Real Time Clock
/// </summary>
public class DS3231 : IDisposable
{
    #region -> Address Constants
    /// <summary>
    /// The register address for temperature data in the DS3231 RTC.
    /// </summary>
    private const byte TEMP_REG = 0x11;

    /// <summary>
    /// The register address for the time in the DS3231 RTC.
    /// </summary>
    private const byte TIME_REG = 0x00;
    #endregion


    #region -> Fields
    /// <summary>
    /// The I2C device used to communicate with the DS3231 RTC.
    /// </summary>
    private readonly I2cDevice _device;

    /// <summary>
    /// The synchronization object used to lock access to the I2C device.
    /// </summary>
    private object _sync = new();
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the DS3231 
    /// class  with the specified device address.
    /// </summary>
    /// <param name="deviceAddress">The device address of the DS3231.</param>
    public DS3231(byte deviceAddress = 0x68)
    {
        lock (_sync)
        {
            var settings = new I2cConnectionSettings(1, deviceAddress);
            _device = I2cDevice.Create(settings);
        }
    }
    #endregion


    #region -> Public Methods
    /// <summary>
    /// Reads the temperature from the DS3231 RTC module.
    /// </summary>
    /// <returns>The temperature in degrees Celsius.</returns>
    public double ReadTemperature()
    {
        lock (_sync)
        {
            Span<byte> data = stackalloc byte[2];
            _device.WriteByte(TEMP_REG);
            _device.Read(data);
            return data[0] + (data[1] >> 6) * 0.25;
        }
    }

    /// <summary>
    /// Reads the current time from the DS3231 RTC module.
    /// </summary>
    /// <returns>The current time as a DateTime object.</returns>
    public DateTime ReadTime()
    {
        lock (_sync)
        {
            Span<byte> data = stackalloc byte[7];
            _device.WriteByte(TIME_REG);
            _device.Read(data);
            return new DateTime(
                BcdToDec(data[6]) + 2000, // Year
                BcdToDec(data[5]), // Month
                BcdToDec(data[4]), // Day
                BcdToDec(data[2]), // Hour
                BcdToDec(data[1]), // Minute
                BcdToDec(data[0]), // Second
                DateTimeKind.Utc
            );
        }
    }

    /// <summary>
    /// Sets the time on the DS3231 RTC module.
    /// </summary>
    /// <param name="dateTime">The current time as a DateTime object.</param>
    public void WriteTime(DateTime dateTime)
    {
        lock (_sync)
        {
            Span<byte> data = stackalloc byte[8];

            // Set the register address to the time register.
            data[0] = TIME_REG;

            // Set the time data in the data buffer.
            data[1] = DecToBcd(dateTime.Second);
            data[2] = DecToBcd(dateTime.Minute);
            data[3] = DecToBcd(dateTime.Hour);
            data[4] = DecToBcd((int)dateTime.DayOfWeek);
            data[5] = DecToBcd(dateTime.Day);
            data[6] = DecToBcd(dateTime.Month);
            data[7] = DecToBcd(dateTime.Year - 2000);

            // Write the time data to the RTC module.
            _device.Write(data);
        }
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Converts a binary-coded decimal (BCD) value to a decimal value.
    /// </summary>
    /// <param name="bcd">The BCD value to convert.</param>
    /// <returns>The decimal value converted from the BCD value.</returns>
    private static int BcdToDec(byte bcd)
    {
        return ((bcd / 16 * 10) + (bcd % 16));
    }

    /// <summary>
    /// Converts a decimal value to binary-coded decimal (BCD) format.
    /// </summary>
    /// <param name="dec">The decimal value to convert.</param>
    /// <returns>The BCD representation of the decimal value.</returns>
    private static byte DecToBcd(int dec)
    {
        return (byte)((dec / 10 * 16) + (dec % 10));
    }

    /// <summary>
    /// Releases the unmanaged resources used by the DS3231 
    /// and optionally releases the managed resources.
    /// </summary>
    public void Dispose()
    {
        lock (_sync)
        {
            _device?.Dispose();
        }
    }
    #endregion
}
