namespace RPIDBClock.NET;

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
    /// Gets the current network time from an NTP server.
    /// </summary>
    /// <returns></returns>
    DateTime GetNetworkTime();
}
