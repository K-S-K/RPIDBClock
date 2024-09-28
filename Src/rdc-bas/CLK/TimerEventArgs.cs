namespace RPIDBClock.CLK;

/// <summary>
/// Represents the event arguments for the timer event.
/// </summary>
/// <param name="time">The time of the event.</param>
public class TimerEventArgs(DateTime time) : EventArgs
{
    /// <summary>
    /// Gets the time of the event.
    /// </summary>
    public DateTime Time { get; init; } = time;

    /// <summary>
    /// Returns a string representation 
    /// of the event arguments for debugging purposes.
    /// </summary>
    public override string ToString() => $"{Time:yyyy.MM.dd HH:mm:ss}";
}
