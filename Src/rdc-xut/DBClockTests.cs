using RPIDBClock.Svc;
using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;

namespace RPIDBClock.Tests;

public class DBClockTests
{
    private readonly LCDStub _lcd = new();
    private readonly RTCStub _rtc = new();
    private readonly NTPStub _ntp = new();


    [Fact]
    public void PrepareDisplayTest()
    {
        // Arrange and Act
        DBClock clock = new(_ntp, _lcd, _rtc);

        // Get the log data from the LCDStub.
        string log = _lcd.Log;

        // Assert
        Assert.Contains("LCDStub: PrepareDisplay()\n", log);
    }
}
