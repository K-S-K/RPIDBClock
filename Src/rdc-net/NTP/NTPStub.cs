namespace RPIDBClock.NET.NTP;

/// <summary>
/// Represents a service for replacing 
/// real interacting with a network 
/// time protocol (NTP) server.
/// </summary>
public class NTPStub : INTPService
{
    /// <summary>
    /// Gets or sets the date and time value.
    /// </summary>
    public DateTime DateTimeValue { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets the current network time.
    /// </summary>
    public DateTime GetNetworkTime() => DateTimeValue;
}
