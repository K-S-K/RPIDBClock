namespace RPIDBClock.Svc;

public interface IDBClock : IDisposable
{
    /// <summary>
    /// Starts the clock.
    /// </summary>
    /// <remarks>
    /// This method gets the current network time and sets it on the RTC module.
    /// It also resumes the timer.
    /// </remarks>
    void Start();

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    void Resume();
}
