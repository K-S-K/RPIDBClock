namespace RPIDBClock.RTC;

/// <summary>
/// Interface for Real Time Clock
/// </summary>
public interface IRTCService
{
    /// <summary>
    /// Reads the temperature from the RTC module.
    /// </summary>
    /// <returns>The temperature in degrees Celsius.</returns>
    double ReadTemperature();

    /// <summary>
    /// Reads the current time from the RTC module.
    /// </summary>
    /// <returns>The current time as a DateTime object.</returns>
    DateTime ReadTime();

    /// <summary>
    /// Sets the current time on the RTC module.
    /// </summary>
    /// <param name="time">The time to set on the RTC module.</param>
    void WriteTime(DateTime time);
}
