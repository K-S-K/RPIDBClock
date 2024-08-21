namespace RPIDBClock.Svc;

/// <summary>
/// Represents the event arguments for the timer event.
/// </summary>
public class TimerEventArgs : EventArgs
{
    /// <summary>
    /// Gets the time of the event.
    /// </summary>
    public  DateTime Time { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimerEventArgs"/> class.
    /// </summary>
    /// <param name="time">The time of the event.</param>
    public TimerEventArgs(DateTime time)
    {
        Time = time;
    }

    /// <summary>
    /// Returns a string representation of the event arguments for debugging purposes.
    /// </summary>
    public override string ToString()=> $"{Time:yyyy.MM.dd HH:mm:ss}";
}
