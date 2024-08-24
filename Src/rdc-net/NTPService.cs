using System.Net;
using System.Net.Sockets;

namespace RPIDBClock.NET;

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
    /// <returns></returns>
    /// <remarks>
    /// This method sends an NTP request to the specified NTP server
    /// and receives an NTP response containing the current network time.
    /// </remarks>
    public DateTime GetNetworkTime()
    {
        lock (_sync)
        {
            return GetNetworkTime(_ntpServer);
        }
    }

    /// <summary>
    /// Gets the current network time from the specified NTP server.
    /// </summary>
    /// <param name="ntpServer">The NTP server to query.</param>
    /// <returns>The current network time.</returns>
    /// <remarks>
    /// This method sends an NTP request to the specified NTP server
    /// and receives an NTP response containing the current network time.
    /// </remarks>
    private DateTime GetNetworkTime(string ntpServer)
    {
        // The NTP data buffer.
        var ntpData = new byte[48];

        // Set the first byte of the NTP data buffer to 0x1B.
        ntpData[0] = 0x1B;

        // Get the IP addresses of the NTP server.
        IPAddress[] addresses = Dns.GetHostAddresses(ntpServer);

        // Create a new IPEndPoint with the first IP address of the NTP server.
        IPEndPoint ipEndPoint = new(addresses[0], 123);

        // Create a new UDP socket.
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
        }

        // Get the current time from the NTP data buffer.
        const byte serverReplyTime = 40;
        ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
        ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        DateTime networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

        return networkDateTime;
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
