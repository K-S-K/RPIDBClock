namespace RPIDBClock.RTC;

/// <summary>
/// Represents a service for replacing real interacting with a real-time clock (RTC).
/// </summary>
public class RTCStub : IRTCService
{
    /// <summary>
    /// Reads the temperature from the RTC module.
    /// </summary>
    /// <returns></returns>
    public double ReadTemperature()
    {
        return 0.0; // Placeholder return value, replace with actual implementation.
    }

    /// <summary>
    /// Reads the current time from the RTC module.
    /// </summary>
    /// <returns></returns>
    public DateTime ReadTime()
    {
        return DateTime.UtcNow; // Placeholder return value, replace with actual implementation.
    }

    /// <summary>
    /// Sets the current time on the RTC module.
    /// </summary>
    /// <param name="time"></param>
    public void WriteTime(DateTime time)
    {
        Console.WriteLine($"RTCStub: WriteTime({time})");
    }
}
