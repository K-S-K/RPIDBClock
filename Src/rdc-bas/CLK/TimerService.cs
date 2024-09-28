namespace RPIDBClock.CLK;

/// <summary>
/// The timer service.
/// </summary>
/// <remarks>
/// This service provides a timer 
/// that triggers an event with 
/// configurable interval.
/// </remarks>
public class TimerService : ITimerService
{
    /// <summary>
    /// Occurs when the timer triggers an event.
    /// </summary>
    public event EventHandler<TimerEventArgs>? TimerEvent;

    #region -> Fields
    /// <summary>
    /// The timer that triggers the clock update.
    /// </summary>
    private Timer? timer;
    #endregion


    #region -> Constructors
    public TimerService(int interval)
    {
        Interval = interval;

        PrepareTimer();
    }
    #endregion


    #region -> Properties
    /// <summary>
    /// Timer interval in milliseconds.
    /// </summary>
    public int Interval { get; init; }
    #endregion


    #region -> Methods
    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause() => timer?.Change(Timeout.Infinite, Timeout.Infinite);

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void Resume() => timer?.Change(0, Interval);

    /// <summary>
    /// Disposes the timer.
    /// </summary>
    public void Dispose()
    {
        timer?.Dispose();
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Prepares the timer.
    /// </summary>
    /// <remarks>
    /// This method initializes the timer and sets the timer interval to 1 second.
    /// </remarks>
    /// 
    private void PrepareTimer()
    {
        // Define the callback method for the timer.
        void callback(object? state)
        {
            DateTime time = DateTime.UtcNow;

            TimerEvent?.Invoke(this, new TimerEventArgs(time));
        }

        // Create a new timer that triggers the callback method every second.
        timer = new Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
    }
    #endregion
}
