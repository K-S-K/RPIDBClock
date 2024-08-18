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
public static class NtpClient
{
    /// <summary>
    /// Gets the current network time from an NTP server.
    /// </summary>
    /// <returns></returns>
    public static DateTime GetNetworkTime()
    {
        // The NTP server to query.
        const string ntpServer = "pool.ntp.org";

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

    /// <summary>
    /// Swaps the endianness of the specified value.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                      ((x & 0x0000ff00) << 8) +
                      ((x & 0x00ff0000) >> 8) +
                      ((x & 0xff000000) >> 24));
    }
}
