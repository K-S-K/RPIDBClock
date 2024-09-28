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
    /// Pauses the timer.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    void Resume();
}
