using System;

namespace RPIDBClock.Log;

/// <summary>
/// Represents a simple logger.
/// </summary>
public interface ISimpleLogger
{
  void AddEvent(LogEvent logEvent);

  /// <summary>
  /// Adds the specified event to the log.
  /// </summary>
  /// <param name="logEventClass">The log event class.</param>
  /// <param name="logEventMethod">The log event method.</param>
  /// <param name="message">The message.</param>
  void AddEvent(LogEventClass logEventClass, LogEventMethod logEventMethod, string message = "");

  /// <summary>
  /// Checks if the log contains the specified event.
  /// </summary>
  /// <param name="logEventClass">The log event class.</param>
  /// <param name="logEventMethod">The log event method.</param>
  /// <param name="message">The message.</param>
  /// <returns>True if the log contains the specified event; otherwise, false.</returns>
  bool ContainsEvent(LogEventClass logEventClass, LogEventMethod logEventMethod, string message = "");
}
