namespace RPIDBClock.NET.VRN;

/// <summary>
/// Single Trip Description
/// </summary>
public record VRNTrainTrip
{
    /// <summary>
    /// The Name Train Route (e.g. S3)
    /// </summary>
    public required string Train { get; init; } = null!;

    /// <summary>
    /// The Destination of the Train 
    /// (e.g. Karlsruhe Hauptbahnhof) - the whole trip name
    /// </summary>
    public required string Destination { get; init; } = null!;

    /// <summary>
    /// The planned Departure Time like in the schedule
    public required DateTime DepartureNormal { get; init; }

    /// <summary>
    /// The planned Arrival Time like in the schedule
    /// </summary>
    public required DateTime ArrivalNormal { get; init; }

    /// <summary>
    /// The expected Departure Time 
    /// regarding the current situation
    /// </summary>
    public required DateTime DepartureExpected { get; init; }

    /// <summary>
    /// The expected Arrival Time 
    /// regarding the current situation
    /// </summary>
    public required DateTime ArrivalExpected { get; init; }

    /// <summary>
    /// The Duration of the Trip
    /// </summary>
    public required TimeSpan Duration { get; init; }

    /// <summary>
    /// The Start Point of the Trip
    /// </summary>
    public required string StartPoint { get; init; } = null!;

    /// <summary>
    /// The End Point of the Trip
    /// </summary>
    public required string EndPoint { get; init; } = null!;

    /// <summary>
    /// The Train Trip string representation for the debug purposes
    /// </summary>
    public override string ToString() => $"{Train} {DepartureExpected:HH:mm} ({Destination}) {StartPoint} - {EndPoint}";
}
