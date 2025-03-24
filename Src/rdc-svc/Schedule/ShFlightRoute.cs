namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// Flight Route Description
/// </summary>
public record ShFlightRoute
{
    /// <summary>
    /// The Origin of the Flight Route (e.g. FRA)
    /// </summary>
    public string Orig { get; init; } = null!;

    /// <summary>
    /// The Destination of the Flight Route (e.g. JFK)
    /// </summary>
    public string Dest { get; init; } = null!;

    /// <summary>
    /// The String representation of the Flight Route for the debug purposes
    /// </summary>
    public override string ToString() => $"{Orig} - {Dest}";

    /// <summary>
    /// The Empty Flight Route Stub
    /// </summary>
    public static ShFlightRoute Empty => new ShFlightRoute { Orig = string.Empty, Dest = string.Empty };
}
