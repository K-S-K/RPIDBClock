using RPIDBClock.Log;
using RPIDBClock.CLK;
using RPIDBClock.Svc;
using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;

namespace RPIDBClock.Tests;

public class DBClockTests
{
    [Fact]
    public void DBClock_01_Ctor_Test()
    {
        ISimpleLogger logger = new SimpleLogger();
        TmrStub _tmr = new(logger);
        LCDStub _lcd = new(logger);
        RTCStub _rtc = new(logger);
        NTPStub _ntp = new();


        // Arrange and Act
        DBClock clock = new(_tmr, _ntp, _lcd, _rtc);

        // Assert
        Assert.True(logger.ContainsEvent(LogEventClass.LCD, LogEventMethod.PrepareDisplay));
    }

    [Fact]
    public void DBClock_02_Resume_Test()
    {
        ISimpleLogger logger = new SimpleLogger();
        TmrStub _tmr = new(logger);
        LCDStub _lcd = new(logger);
        RTCStub _rtc = new(logger);
        NTPStub _ntp = new();

        // Arrange
        DBClock clock = new(_tmr, _ntp, _lcd, _rtc);

        // Act

        // Resume the clock.
        clock.Resume();

        // Assert
        Assert.True(logger.ContainsEvent(LogEventClass.LCD, LogEventMethod.PrepareDisplay));
        Assert.True(logger.ContainsEvent(LogEventClass.Timer, LogEventMethod.Resume));
    }
}