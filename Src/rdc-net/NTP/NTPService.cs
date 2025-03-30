using System.Net;
using System.Net.Sockets;

namespace RPIDBClock.NET.NTP;

/// <summary>
/// Represents an NTP client.
/// </summary>
/// <remarks>
/// This class provides functionality
/// to retrieve the current network time from an NTP server.
/// </remarks>
public class NTPService : INTPService
{
    #region -> Fields
    /// <summary>
    /// The NTP server to query.
    /// </summary>
    /// <remarks>
    /// This field specifies the NTP server to query
    /// when retrieving the current network time.
    /// </remarks>
    private readonly string _ntpServer;

    /// <summary>
    /// The synchronization object used to lock access to the NTP server.
    /// </summary>
    private object _sync = new();
    #endregion

    /// <summary>
    /// The event for returning log messages.
    /// </summary>
    public event EventHandler<string>? LogEvent;

    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="NTPService"/> class.
    /// </summary>
    /// <param name="ntpServer">The NTP server to query.</param>
    /// <remarks>
    /// This constructor initializes a new instance of the NTP client
    /// with the specified NTP server to query.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when the NTP server is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when the NTP server is invalid.</exception>
    /// <remarks>
    public NTPService(string ntpServer = "pool.ntp.org")
    {
        lock (_sync)
        {
            ArgumentNullException.ThrowIfNull(ntpServer, nameof(ntpServer));
            ArgumentException.ThrowIfNullOrWhiteSpace(ntpServer, nameof(ntpServer));

            _ntpServer = ntpServer;
        }
    }
    #endregion


    #region -> Methods
    /// <summary>
    /// Gets the current network time from an NTP server.
    /// </summary>
    /// <param name="networkDateTime">The current network time.</param>
    /// <returns>True if the network time was successfully retrieved; otherwise, false.</returns>
    /// <remarks>
    /// This method sends an NTP request to the specified NTP server
    /// and receives an NTP response containing the current network time.
    /// </remarks>
    public bool GetNetworkTime(out DateTime networkDateTime)
    {
        lock (_sync)
        {
            return GetNetworkTime(_ntpServer, out networkDateTime);
        }
    }

    /// <summary>
    /// Gets the current network time from the specified NTP server.
    /// </summary>
    /// <param name="ntpServer">The NTP server to query.</param>
    /// <param name="networkDateTime">The current network time.</param>
    /// <returns>True if the network time was successfully retrieved; otherwise, false.</returns>
    /// <remarks>
    /// This method sends an NTP request to the specified NTP server
    /// and receives an NTP response containing the current network time.
    /// </remarks>
    private bool GetNetworkTime(string ntpServer, out DateTime networkDateTime)
    {
        // Log the NTP server.
        LogEvent?.Invoke(this, $"Srv: {ntpServer}");

        // Get the IP addresses of the NTP server.
        IPAddress[] addresses;
        try
        {
            addresses = Dns.GetHostAddresses(ntpServer);
        }
        catch (SocketException ex)
        {
            LogEvent?.Invoke(this, $"Err: {ex.Message}");
            networkDateTime = DateTime.MinValue;
            return false;
        }

        // Log the first IP address of the NTP server.
        LogEvent?.Invoke(this, $"Addr: {addresses[0]}");

        // Create a new IPEndPoint with the first IP address of the NTP server.
        IPEndPoint ipEndPoint = new(addresses[0], 123);

        // The NTP data buffer.
        var ntpData = new byte[48];

        // Set the first byte of the NTP data buffer to 0x1B.
        ntpData[0] = 0x1B;

        // Get the current time from the NTP server.
        try
        {
            // Create a new UDP socket.
            using var socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
        }
        catch (Exception ex)
        {
            LogEvent?.Invoke(this, $"Err: {ex.Message}");
            networkDateTime = DateTime.MinValue;
            return false;
        }


        // Get the current time from the NTP data buffer.
        const byte serverReplyTime = 40;
        ulong integerPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
        ulong fractionPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        integerPart = SwapEndianness(integerPart);
        fractionPart = SwapEndianness(fractionPart);

        var milliseconds = (integerPart * 1000) + (fractionPart * 1000 / 0x100000000L);
        networkDateTime = new DateTime(1900, 1, 1).AddMilliseconds((long)milliseconds);

        return true;
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Swaps the endianness of the specified value.
    /// </summary>
    private static ulong SwapEndianness(ulong x)
    => (x & 0x000000FFU) << 24 |
       (x & 0x0000FF00U) << 8 |
       (x & 0x00FF0000U) >> 8 |
       (x & 0xFF000000U) >> 24;
    #endregion
}
