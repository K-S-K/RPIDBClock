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

    /// <summary>
    /// The expiration time of the schedule
    /// </summary>
    private DateTime _expirationTime = DateTime.MinValue;


    /// <summary>
    /// The last requested route for the cache
    /// </summary>
    private ShFlightRoute? LastRequestedRoute;

    /// <summary>
    /// The flights of last response for the cache
    /// </summary>
    private IReadOnlyList<ShFlightItem>? LastResponseFlights;
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

    /// <summary>
    /// The expiration time of the last retrieved schedule
    /// </summary>
    /// <remarks>
    /// TODO: Use it
    /// </remarks>
    [JsonIgnore]
    public DateTime ExpirationTime
    {
        get
        {
            if (_expirationTime == DateTime.MinValue)
            {
                _expirationTime = GetMinDepartureDate();
            }

            return _expirationTime;
        }
    }
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
    /// Checks if the last response is expired
    /// </summary>
    /// <param name="route">The route to check the response for</param>
    /// <param name="from">The date and time to check from</param>
    /// <returns>True if the last response is expired, otherwise false</returns>
    public bool IsLastResponseExpired(ShFlightRoute route, DateTime from)
    {
        if (_expirationTime > DateTime.MinValue)
        {
            if (from < _expirationTime)
            {
                if (LastRequestedRoute == route)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Gets the flights from the specified date and time
    /// </summary>
    /// <param name="route">The route to get the flights for</param>
    /// <param name="from">The date and time to start from</param>
    /// <param name="count">The count of the flights to get</param>
    /// <returns>The list of the flights</returns>
    public IReadOnlyList<ShFlightItem> GetFlights(ShFlightRoute route, DateTime from, int count = 2)
    {
        // If the last response is not expired
        if (!IsLastResponseExpired(route, from))
        {
            return LastResponseFlights ?? [];
        }

        // Get the schedule
        if (TryGetRouteSchedule(route, out ShRouteSchedule? schedule))
        {
            var result = schedule?.GetFlights(from, count) ?? [];

            // If there are flights, 
            // set the expiration time
            if (result.Count > 0)
            {
                _expirationTime =
                    result.Min(f => f.DepartureNormal);
            }

            LastRequestedRoute = route;
            LastResponseFlights = result;

            return result;
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

        // Reset the expiration time
        _expirationTime = date;
    }
    #endregion


    #region -> Overrides
    /// <summary>
    /// The string representation of the Global Schedule for the debug purposes
    /// </summary>
    public override string ToString() => $"{Count} routes";
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

    /// <summary>
    /// Gets the minimum departure date of the flights in the schedule
    /// </summary>
    /// <returns>The minimum departure date</returns>
    private DateTime GetMinDepartureDate()
    {
        DateTime minDate = DateTime.MaxValue;
        foreach (ShRouteSchedule schedule in _routes.Values)
        {
            DateTime date = schedule.Flights
                .Select(f => f.DepartureNormal.Date).Min();

            if (date < minDate)
            {
                minDate = date;
            }
        }

        return minDate;
    }
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
