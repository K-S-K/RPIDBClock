using System.Diagnostics;

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

    /// <summary>
    /// The event for returning log messages.
    /// </summary>
    public event EventHandler<string>? LogEvent;

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

    /// <summary>
    /// Sets the system time.
    /// </summary>
    /// <param name="utcTime">The time to set.</param>
    public void SetSystemTime(DateTime utcTime)
    {
        bool isProd = (Environment.GetEnvironmentVariable(
            "ASPNETCORE_ENVIRONMENT") ?? "Production") == "Production";

        if (!isProd)
        {
            LogEvent?.Invoke(this, $"Skip SetSystemTime");
            return;
        }

        try
        {
            // Command to set system time in UTC
            string command = $"sudo date -u -s '{utcTime:yyyy-MM-dd HH:mm:ss}'";

            ProcessStartInfo psi = new()
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new() { StartInfo = psi };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                LogEvent?.Invoke(this, error);
            }
            else
            {
                LogEvent?.Invoke(this, $"Success: {output}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            LogEvent?.Invoke(this, $"Exception: {ex.Message}");
        }
    }

    public DateTime GetSystemTime() => DateTime.UtcNow;
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
