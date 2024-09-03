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
    /// Shifts the flight to the specified date
    /// </summary>
    /// <param name="date">The date to shift the flight to</param>
    /// <returns>The shifted flight</returns>
    public ShFlightItem ShiftToDate(DateTime date) => new()
    {
        Name = Name,
        DepartureNormal = date.Add(DepartureNormal.TimeOfDay),
        ArrivalNormal = date.Add(ArrivalNormal.TimeOfDay),
        DepartureExpected = date.Add(DepartureExpected.TimeOfDay),
        ArrivalExpected = date.Add(ArrivalExpected.TimeOfDay),
        Duration = Duration
    };

    /// <summary>
    /// The string representation of the Particular Flight for the debug purposes
    /// </summary>
    public override string ToString()
        => $"{Name} {DepartureExpected:HH:mm} - {ArrivalExpected:HH:mm}";
}
