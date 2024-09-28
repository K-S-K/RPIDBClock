using System.Text;

namespace RPIDBClock.Log;

/// <summary>
/// Represents a simple logger.
/// </summary>
public class SimpleLogger : ISimpleLogger
{
    /// <summary>
    /// Log data collector.
    /// </summary>
    /// <remarks>
    /// This field collects the log data during the interaction with the LCD display.
    /// </remarks>
    private readonly StringBuilder _log = new();

    /// <summary>
    /// The log data collected during the interaction with the LCD display.
    /// </summary>
    /// <remarks>
    /// This property provides the log data collected during the interaction with the LCD display.
    /// </remarks>
    public string Log => _log.ToString();

    /// <summary>
    /// Writes the specified message to the console and the log.
    /// </summary>
    public void AddLine(string message)
    {
        _log.AppendLine(message);
        Console.WriteLine(message);
    }

    /// <summary>
    /// Adds a separator to the log.
    /// </summary>
    public void AddLogSeparator()
        => AddLine("--------------------------------------------------");

    /// <summary>
    /// Adds a separator with the specified message to the log.
    /// </summary>
    public void AddLogSeparator(string message)
        => AddLine($"--------------------------------------- {message}");

}
