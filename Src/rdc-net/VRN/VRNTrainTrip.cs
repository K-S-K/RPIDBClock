namespace RPIDBClock.NET.VRN;

/// <summary>
/// Single Trip Description
/// </summary>
public class VRNTrainTrip
{
    /// <summary>
    /// The Name Train Route (e.g. S3)
    /// </summary>
    public string Train { get; set; } = null!;

    /// <summary>
    /// The Destination of the Train 
    /// (e.g. Karlsruhe Hauptbahnhof) - the whole trip name
    /// </summary>
    public string Destination { get; set; } = null!;

    /// <summary>
    /// The planned Departure Time like in the schedule
    public DateTime DepartureNormal { get; set; }

    /// <summary>
    /// The planned Arrival Time like in the schedule
    /// </summary>
    public DateTime ArrivalNormal { get; set; }

    /// <summary>
    /// The expected Departure Time 
    /// regarding the current situation
    /// </summary>
    public DateTime DepartureExpected { get; set; }

    /// <summary>
    /// The expected Arrival Time 
    /// regarding the current situation
    /// </summary>
    public DateTime ArrivalExpected { get; set; }

    /// <summary>
    /// The Duration of the Trip
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// The Start Point of the Trip
    /// </summary>
    public string StartPoint { get; set; } = null!;

    /// <summary>
    /// The End Point of the Trip
    /// </summary>
    public string EndPoint { get; set; } = null!;

    /// <summary>
    /// The Train Trip string representation for the debug purposes
    /// </summary>
    public override string ToString() => $"{Train} {DepartureExpected:HH:mm} ({Destination}) {StartPoint} - {EndPoint}";
}
