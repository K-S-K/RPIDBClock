using System;
using RPIDBClock.CLK;

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
}
