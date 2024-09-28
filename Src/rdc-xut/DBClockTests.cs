using RPIDBClock.Log;
using RPIDBClock.Svc;
using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;

namespace RPIDBClock.Tests;

public class DBClockTests
{
    [Fact]
    public void PrepareDisplayTest()
    {
        ISimpleLogger logger = new SimpleLogger();
        LCDStub _lcd = new(logger);
        RTCStub _rtc = new(logger);
        NTPStub _ntp = new();


        // Arrange and Act
        DBClock clock = new(_ntp, _lcd, _rtc);

        // Get the log data from the LCDStub.
        string log = logger.Log;

        // Assert
        Assert.Contains("LCDStub: PrepareDisplay()\n", log);
    }
}
