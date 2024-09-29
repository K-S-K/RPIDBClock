using RPIDBClock.Log;

namespace RPIDBClock.CLK;

/// <summary>
/// Represents a stub timer service for replacing real one.
/// </summary>
/// <param name="logger"></param>
public class TmrStub(ISimpleLogger logger) : ITimerService
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ISimpleLogger _logger = logger;


    /// <summary>
    /// Emulates the timer event 
    /// when user class calls the FireTimerEvent method.
    /// </summary>
    public event EventHandler<TimerEventArgs>? TimerEvent;

    /// <summary>
    /// Emulates the timer event
    /// </summary>
    public void FireTimerEvent()
    {
        _logger.AddLine("TimerStub: FireTimerEvent()");
        TimerEvent?.Invoke(this, new TimerEventArgs(DateTime.MinValue));
    }

    /// <summary>
    /// Registers The Dispose method call.
    public void Dispose()
        => _logger.AddLine("TimerStub: Dispose()");

    /// <summary>
    /// Registers the Pause method call.
    /// </summary>
    public void Pause()
        => _logger.AddLine("TimerStub: Pause()");

    /// <summary>
    /// Registers the Resume method call.
    /// </summary>
    public void Resume()
        => _logger.AddLine("TimerStub: Resume()");
}
