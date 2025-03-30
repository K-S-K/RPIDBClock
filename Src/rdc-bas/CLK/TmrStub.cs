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
    /// The event for returning log messages.
    /// </summary>
    public event EventHandler<string>? LogEvent;

    /// <summary>
    /// Emulates the timer event
    /// </summary>
    public void FireTimerEvent()
    {
        _logger.AddEvent(LogEventClass.Timer, LogEventMethod.FireTimerEvent);
        TimerEvent?.Invoke(this, new TimerEventArgs(DateTime.MinValue));
    }

    /// <summary>
    /// Registers The Dispose method call.
    public void Dispose()
        => _logger.AddEvent(LogEventClass.Timer, LogEventMethod.Dispose);

    /// <summary>
    /// Registers the Pause method call.
    /// </summary>
    public void Pause()
        => _logger.AddEvent(LogEventClass.Timer, LogEventMethod.Pause);

    /// <summary>
    /// Registers the Resume method call.
    /// </summary>
    public void Resume()
        => _logger.AddEvent(LogEventClass.Timer, LogEventMethod.Resume);

    /// <summary>
    /// Sets the system time.
    /// </summary>
    /// <param name="utcTime">The time to set.</param>
    public void SetSystemTime(DateTime utcTime)
    {
        _logger.AddEvent(LogEventClass.Timer,
            LogEventMethod.SetSystemTime,
            $"Time: {utcTime}");

        LogEvent?.Invoke(this, $"SetSystemTime Stub: {utcTime}");
    }
}
