namespace RPIDBClock.Log;

public enum LogEventClass
{
    /// <summary>
    /// Represents the LCD class.
    /// </summary>
    LCD,

    /// <summary>
    /// Represents the Runtime Clock class.
    RTC,

    /// <summary>
    /// Represents the Timer class.
    /// </summary>
    Timer,

    /// <summary>
    /// Represents the DBClock class.
    /// </summary>
    DBClock
}

public enum LogEventMethod
{
    /// <summary>
    /// Represents the PrepareDisplay method.
    /// </summary>
    PrepareDisplay,

    /// <summary>
    /// Represents the Clear method.
    /// </summary>
    Clear,

    /// <summary>
    /// Represents the Write method.
    /// </summary>
    Write,

    /// <summary>
    /// Represents the CreateCustomCharacter method.
    /// </summary>
    CreateCustomCharacter,

    /// <summary>
    /// Represents the SetBrightness method.
    /// </summary>
    SetBrightness,

    /// <summary>
    /// Represents the FireTimerEvent method.
    /// </summary>
    FireTimerEvent,

    /// <summary>
    /// Represents the Dispose method.
    /// </summary>
    Dispose,

    /// <summary>
    /// Represents the Pause method.
    /// </summary>
    Pause,

    /// <summary>
    /// Represents the Resume method.
    /// </summary>
    Resume,

    /// <summary>
    /// Represents the WriteTime method.
    /// </summary>
    WriteTime
}
