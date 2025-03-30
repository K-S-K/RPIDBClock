namespace RPIDBClock.NET.NTP;

/// <summary>
/// Represents an NTP client.
/// </summary>
/// <remarks>
/// This class provides functionality
/// to retrieve the current network time from an NTP server.
/// </remarks>
public interface INTPService
{
    /// <summary>
    /// The event for returning log messages.
    /// </summary>
    event EventHandler<string>? LogEvent;

    /// <summary>
    /// Gets the current network time from an NTP server.
    /// </summary>
    /// <param name="networkDateTime">The current network time.</param>
    /// <returns>True if the network time was successfully retrieved; otherwise, false.</returns>
    /// <remarks>
    /// This method sends an NTP request to the specified NTP server
    /// and receives an NTP response containing the current network time.
    /// </remarks>
    bool GetNetworkTime(out DateTime networkDateTime);
}
