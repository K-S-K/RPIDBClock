using System.Text.Json.Serialization;

namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// Particular Flight Schedule
/// </summary>
public class ShRouteSchedule
{
    #region -> Data
    private readonly List<ShFlightItem> _flights = [];
    #endregion


    #region -> Properties
    /// <summary>
    /// The Route of the Schedule
    /// </summary>
    [JsonPropertyName("Route")]
    public ShFlightRoute Route { get; init; } = null!;

    /// <summary>
    /// The list of the flights in the schedule
    /// </summary>
    [JsonPropertyName("Flights")]
    public List<ShFlightItem> Flights
    {
        get => _flights;
        set
        {
            _flights.Clear();
            _flights.AddRange(value);
        }
    }

    /// <summary>
    /// The count of the flights in the schedule
    /// </summary>
    [JsonIgnore]
    public int Count => Flights.Count;
    #endregion


    #region -> Methods
    /// <summary>
    /// Adds the flight to the schedule
    /// </summary>
    /// <param name="flight">The flight to add</param>
    public void AddFlight(ShFlightItem flight)
    {
        Flights.Add(flight);
    }

    /// <summary>
    /// Gets the flights from the specified date and time
    /// </summary>
    /// <param name="from">The date and time to start from</param>
    /// <param name="count">The count of the flights to get</param>
    /// <returns>The list of the flights</returns>
    /// <remarks>
    /// The method returns the list of the flights starting from the specified date and time.
    /// The count of the flights to get can be specified.
    /// </remarks>
    public List<ShFlightItem> GetFlights(DateTime from, int count = 2)
    {
        return Flights.Where(f => f.DepartureExpected >= from)
            .OrderBy(f => f.DepartureExpected)
            .Take(count)
            .ToList();
    }

    /// <summary>
    /// The string representation of the Route Schedule for the debug purposes
    /// </summary>
    public override string ToString() => $"{Route} {Count} flights";
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the route schedule.
    /// </summary>
    public ShRouteSchedule(ShFlightRoute route)
    {
        Route = route;
    }
    #endregion
}
