using System.Text.Json.Serialization;

namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// Global Schedule
/// </summary>
public class ShGlobalSchedule
{
    #region -> Data
    /// <summary>
    /// The list of the routes
    /// </summary>
    private readonly Dictionary<ShFlightRoute, ShRouteSchedule> _routes = [];
    #endregion


    #region -> Properties
    /// <summary>
    /// Gets the whole list of the route schedules
    [JsonPropertyName("Routes")]
    public List<ShRouteSchedule> Routes
    {
        get => _routes.Values.ToList();
        set
        {
            _routes.Clear();
            foreach (ShRouteSchedule schedule in value)
            {
                _routes.Add(schedule.Route, schedule);
            }
        }
    }

    /// <summary>
    /// The count of the routes in the schedule
    /// </summary>
    [JsonIgnore]
    public int Count => _routes.Count;
    #endregion


    #region -> Methods
    /// <summary>
    /// Adds the flight to the schedule
    /// </summary>
    /// <param name="route">The route to add the flight to</param>
    /// <param name="flight">The flight to add</param>
    public void AddFlight(ShFlightRoute route, ShFlightItem flight)
    {
        if (!TryGetRouteSchedule(route, out ShRouteSchedule? schedule))
        {
            schedule = new(route);
            _routes.Add(route, schedule);
        }

        schedule?.AddFlight(flight);
    }

    /// <summary>
    /// Gets the flights from the specified date and time
    /// </summary>
    /// <param name="route">The route to get the flights for</param>
    /// <param name="from">The date and time to start from</param>
    /// <param name="count">The count of the flights to get</param>
    /// <returns>The list of the flights</returns>
    /// 
    public IReadOnlyList<ShFlightItem> GetFlights(ShFlightRoute route, DateTime from, int count = 2)
    {
        if (TryGetRouteSchedule(route, out ShRouteSchedule? schedule))
        {
            return schedule?.GetFlights(from, count) ?? [];
        }

        return [];
    }

    /// <summary>
    /// Shifts the schedule to the specified date
    /// </summary>
    /// <param name="date">The date to shift the schedule to</param>
    public void ShiftToDate(DateTime date)
    {
        // Ensure the date is the date only
        date = date.Date;

        // Shift all the routes
        foreach (ShRouteSchedule schedule in _routes.Values)
        {
            schedule.ShiftToDate(date);
        }
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Tries to get the route schedule
    /// </summary>
    /// <param name="route">The route to get the schedule for</param>
    /// <param name="schedule">The schedule of the route</param>
    /// <returns>True if the route schedule is found, otherwise false</returns>    
    private bool TryGetRouteSchedule(ShFlightRoute route, out ShRouteSchedule? schedule)
        => _routes.TryGetValue(route, out schedule);
    #endregion


    #region -> Overrides
    /// <summary>
    /// The string representation of the Global Schedule for the debug purposes
    /// </summary>
    public override string ToString() => $"{Count} routes";
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the global schedule.
    /// </summary>
    /// 
    public ShGlobalSchedule()
    {
    }
    #endregion
}
