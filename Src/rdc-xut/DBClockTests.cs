using RPIDBClock.Log;
using RPIDBClock.CLK;
using RPIDBClock.Svc;
using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;
using RPIDBClock.Svc.Schedule;

namespace RPIDBClock.Tests;

public class DBClockTests
{
    [Fact]
    public void DBClock_01_Ctor_Test()
    {
        IScheduleService _sch = new ScheduleStub();
        ISimpleLogger logger = new SimpleLogger();
        TmrStub _tmr = new(logger);
        LCDStub _lcd = new(logger);
        RTCStub _rtc = new(logger);
        NTPStub _ntp = new();


        // Arrange and Act
        DBClock clock = new(_tmr, _ntp, _lcd, _rtc, _sch);

        // Assert
        Assert.True(logger.ContainsEvent(LogEventClass.LCD, LogEventMethod.PrepareDisplay));
    }

    [Fact]
    public void DBClock_02_Resume_Test()
    {
        IScheduleService _sch = new ScheduleStub();
        ISimpleLogger logger = new SimpleLogger();
        TmrStub _tmr = new(logger);
        LCDStub _lcd = new(logger);
        RTCStub _rtc = new(logger);
        NTPStub _ntp = new();

        // Arrange
        DBClock clock = new(_tmr, _ntp, _lcd, _rtc, _sch);

        // Act

        // Resume the clock.
        clock.Resume();

        // Assert
        Assert.True(logger.ContainsEvent(LogEventClass.LCD, LogEventMethod.PrepareDisplay));
        Assert.True(logger.ContainsEvent(LogEventClass.Timer, LogEventMethod.Resume));
    }
}