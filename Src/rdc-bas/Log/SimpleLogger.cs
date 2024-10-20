namespace RPIDBClock.Log;

/// <summary>
/// Represents a simple logger.
/// </summary>
public class SimpleLogger : ISimpleLogger
{
    /// <summary>
    /// The log events collection.
    /// </summary>
    private SortedDictionary<int, LogEvent> _logEvents = [];

    /// <summary>
    /// Adds the specified event to the log.
    /// </summary>
    /// <param name="logEvent">The log event.</param>
    public void AddEvent(LogEvent logEvent)
    {
        Console.WriteLine($"{logEvent}");
        _logEvents.Add(_logEvents.Count, logEvent);
    }

    /// <summary>
    /// Adds the specified event to the log.
    /// </summary>
    /// <param name="logEventClass">The log event class.</param>
    /// <param name="logEventMethod">The log event method.</param>
    /// <param name="message">The message.</param>
    public void AddEvent(LogEventClass logEventClass, LogEventMethod logEventMethod, string message = "")
        => AddEvent(new LogEvent(logEventClass, logEventMethod, message));

    /// <summary>
    /// Checks if the log contains the specified event.
    /// </summary>
    /// <param name="logEventClass">The log event class.</param>
    /// <param name="logEventMethod">The log event method.</param>
    /// <param name="message">The message.</param>
    /// <returns>True if the log contains the specified event; otherwise, false.</returns>
    public bool ContainsEvent(LogEventClass logEventClass, LogEventMethod logEventMethod, string message = "")
    {
        foreach (var logEvent in _logEvents)
        {
            if (logEvent.Value.LogEventClass == logEventClass &&
                logEvent.Value.LogEventMethod == logEventMethod &&
                logEvent.Value.Message == message)
            {
                return true;
            }
        }

        return false;
    }
}
