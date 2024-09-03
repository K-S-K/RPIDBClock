using System.Text.Json.Serialization;

namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// Particular Flight Description
/// </summary>
public record ShFlightItem
{
    /// <summary>
    /// The Name of the Flight Route (e.g. LH1234)
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The planned Departure Time like in the schedule
    /// </summary>
    [JsonPropertyName("DepNormal")]
    public DateTime DepartureNormal { get; init; }

    /// <summary>
    /// The planned Arrival Time like in the schedule
    /// </summary>
    [JsonPropertyName("ArrNormal")]
    public DateTime ArrivalNormal { get; init; }

    /// <summary>
    /// The expected Departure Time 
    /// regarding the current situation
    /// </summary>
    [JsonPropertyName("DepExpected")]
    public DateTime DepartureExpected { get; init; }

    /// <summary>
    /// The expected Arrival Time 
    /// regarding the current situation
    /// </summary>
    [JsonPropertyName("ArrExpected")]
    public DateTime ArrivalExpected { get; init; }

    /// <summary>
    /// The Duration of the Trip
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// The string representation of the Particular Flight for the debug purposes
    /// </summary>
    public override string ToString()
        => $"{Name} {DepartureExpected:HH:mm} - {ArrivalExpected:HH:mm}";
}
