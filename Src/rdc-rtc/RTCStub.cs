using RPIDBClock.Log;

namespace RPIDBClock.RTC;

/// <summary>
/// Represents a service for replacing real interacting with a real-time clock (RTC).
/// </summary>
public class RTCStub(ISimpleLogger logger) : IRTCService
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ISimpleLogger _logger = logger;


    #region -> Test Support
    /// <summary>
    /// Gets or sets the date and time value.
    /// </summary>
    public DateTime DateTimeValue { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the temperature value.
    /// </summary>
    public double TemperatureValue { get; set; } = 0.0;
    #endregion


    #region -> IRTCService Implementation
    /// <summary>
    /// Simulates reading the current temperature from the RTC module.
    /// </summary>
    public double ReadTemperature() => TemperatureValue;

    /// <summary>
    /// Simulates reading the current time from the RTC module.
    /// </summary>
    public DateTime ReadTime() => DateTimeValue;

    /// <summary>
    /// Simulates writing the specified time to the RTC module.
    /// </summary>
    /// <param name="time"></param>
    public void WriteTime(DateTime time)
    {
        DateTimeValue = time;
        _logger.AddEvent(
            LogEventClass.RTC, LogEventMethod.WriteTime,
            $"Time: {time:yyyy.MM.dd HH:mm:ss}");
    }
    #endregion
}
