namespace RPIDBClock.Log;

public record LogEvent(LogEventClass LogEventClass, LogEventMethod LogEventMethod, string Message)
{
    public override string ToString()
        => $"{LogEventClass}.{LogEventMethod}(){(string.IsNullOrWhiteSpace(Message) ? string.Empty : $": {Message}")}";
};
