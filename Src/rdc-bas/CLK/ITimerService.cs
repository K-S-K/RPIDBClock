namespace RPIDBClock.CLK;

/// <summary>
/// The timer service.
/// </summary>
/// <remarks>
/// This service provides a timer that triggers an event every second.
/// </remarks>
public interface ITimerService : IDisposable
{
    /// <summary>
    /// Occurs when the timer triggers an event.
    /// </summary>
    event EventHandler<TimerEventArgs>? TimerEvent;

    /// <summary>
    /// The event for returning log messages.
    /// </summary>
    event EventHandler<string>? LogEvent;

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    void Resume();

    /// <summary>
    /// Sets the system time.
    /// </summary>
    /// <param name="utcTime">The system time in UTC.</param>
    void SetSystemTime(DateTime utcTime);
}
