namespace RPIDBClock.NET.NTP;

/// <summary>
/// Represents a service for replacing 
/// real interacting with a network 
/// time protocol (NTP) server.
/// </summary>
public class NTPStub : INTPService
{
    /// <summary>
    /// The event for returning log messages.
    /// </summary>
    public event EventHandler<string>? LogEvent;

    /// <summary>
    /// Gets or sets the date and time value.
    /// </summary>
    public DateTime DateTimeValue { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets the current network time.
    /// </summary>
    /// <param name="networkDateTime">The current network time.</param>
    /// <returns>True if the network time was successfully retrieved; otherwise, false.</returns>
    public bool GetNetworkTime(out DateTime networkDateTime)
    {
        LogEvent?.Invoke(this, "Getting Stub time...");
        networkDateTime = DateTimeValue;
        return true;
    }
}
