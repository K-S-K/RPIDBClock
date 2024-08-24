namespace RPIDBClock.RTC;

/// <summary>
/// Represents a service for interacting with a real-time clock (RTC).
/// </summary>
/// <remarks>
/// This class provides methods for reading the temperature and time from the RTC, as well as setting the time on the RTC.
/// </remarks>
public class RTCService : IRTCService
{
    #region -> Fields
    /// <summary>
    /// The RTC device used to communicate with the RTC module.
    /// </summary>
    private readonly DS3231 _rtc;
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="RTCService"/> class with the specified device address.
    /// </summary>
    /// <param name="deviceAddress">The device address of the RTC module.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the device address is invalid.</exception>
    /// <exception cref="PlatformNotSupportedException">Thrown when the platform is not supported.</exception>
    /// <exception cref="NotSupportedException">Thrown when the device is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the device access is denied.</exception>
    /// <exception cref="Exception">Thrown when an unknown error occurs.</exception>
    /// <remarks>
    public RTCService(byte deviceAddress)
    {
        _rtc = new DS3231(deviceAddress);
    }
    #endregion


    #region -> Methods
    /// <summary>
    /// Reads the temperature from the RTC module.
    /// </summary>
    /// <returns>The temperature in degrees Celsius.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while reading the temperature.</exception>
    /// <remarks>
    /// The temperature is read from the RTC module and returned as a double value in degrees Celsius.
    /// </remarks>
    public double ReadTemperature() => _rtc.ReadTemperature();

    /// <summary>
    /// Reads the current time from the RTC module.
    /// </summary>
    /// <returns>The current time as a DateTime object.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while reading the time.</exception>
    /// <remarks>
    public DateTime ReadTime() => _rtc.ReadTime();

    /// <summary>
    /// Sets the current time on the RTC module.
    /// </summary>
    /// <param name="time">The time to set on the RTC module.</param>
    /// <exception cref="Exception">Thrown when an error occurs while setting the time.</exception>
    /// <remarks>
    public void WriteTime(DateTime time) => _rtc.WriteTime(time);
    #endregion
}
