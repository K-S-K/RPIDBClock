namespace RPIDBClock.Svc;

public interface IDBClock : IDisposable
{
    /// <summary>
    /// Loads the schedule and  other preparations.
    /// </summary>
    void Prepare();

    /// <summary>
    /// Synchronizes the RTC time 
    // and the system time with 
    // the network time.
    /// </summary>
    /// <param name="withNetworkTime">Indicates 
    // whether to synchronize the system time 
    // with the network time.</param>
    void SyncSystemTime(bool withNetworkTime);

    /// <summary>
    /// Starts the clock.
    /// </summary>
    /// <remarks>
    /// This method gets the current network 
    // time and sets it on the RTC module.
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
