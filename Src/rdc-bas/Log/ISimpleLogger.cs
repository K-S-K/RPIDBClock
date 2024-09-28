using System;

namespace RPIDBClock.Log;

/// <summary>
/// Represents a simple logger.
/// </summary>
public interface ISimpleLogger
{
    /// <summary>
    /// The log data collected during the interaction with the LCD display.
    /// </summary>
    /// <remarks>
    /// This property provides the log data collected during the interaction with the LCD display.
    /// </remarks>
    string Log { get; }

    /// <summary>
    /// Writes the specified message to the console and the log.
    /// </summary>
    void AddLine(string message);

    /// <summary>
    /// Adds an empty separator to the log.
    /// </summary>
    void AddLogSeparator();

    /// <summary>
    /// Adds a separator with the specified message to the log.
    /// </summary>
    void AddLogSeparator(string message);
}
