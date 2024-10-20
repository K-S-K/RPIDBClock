using RPIDBClock.CLK;
using RPIDBClock.Log;

namespace RPIDBClock.Tests;

public class TimerServiceTests
{
    [Fact]
    public async void TimerServiceTest()
    {
        int tickCount = 0;
        int period = 50;
        int delay = (int)(2.2 * period);

        // Arrange
        using TimerService timerService = new(period);

        timerService.TimerEvent += (sender, e) =>
        {
            tickCount++;
        };

        // Acts and Asserts
        await Task.Delay(delay);
        Assert.True(tickCount == 0);

        timerService.Resume();
        await Task.Delay(delay);
        Assert.Equal(3, tickCount);

        timerService.Pause();
        await Task.Delay(delay);
        Assert.Equal(3, tickCount);

        timerService.Resume();
        await Task.Delay(delay);
        Assert.Equal(6, tickCount);
    }

    [Fact]
    public void TmrStubFireEventTest()
    {
        // Arrange
        ISimpleLogger logger = new SimpleLogger();
        TmrStub tmrStub = new(logger);

        // Act
        tmrStub.FireTimerEvent();

        // Assert
        Assert.True(logger.ContainsEvent(LogEventClass.Timer, LogEventMethod.FireTimerEvent));
    }


    [Fact]
    public void TmrStubPauseAndResumeTest()
    {
        // Arrange
        ISimpleLogger logger = new SimpleLogger();
        TmrStub tmrStub = new(logger);

        // Act
        tmrStub.Pause();
        tmrStub.Resume();

        // Assert
        Assert.True(logger.ContainsEvent(LogEventClass.Timer, LogEventMethod.Pause));
        Assert.True(logger.ContainsEvent(LogEventClass.Timer, LogEventMethod.Resume));
    }
}
