using System.Text.Json;

namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// The Schedule Service
/// </summary>
public class ScheduleService : IScheduleService
{
    #region -> Fields
    private ShGlobalSchedule _schedule = null!;

    private ShFlightRoute _route = ShFlightRoute.Empty;

    /// <summary>
    /// The current day.
    /// </summary>
    private int _day = 0;
    #endregion


    #region -> Properties
    /// <summary>
    /// The flight route
    /// </summary>
    public ShFlightRoute Route
    {
        get => _route;
        set => _route = value;
    }
    #endregion


    #region -> Constructors
    /// <summary>
    /// The Schedule Service
    /// </summary>
    public ScheduleService()
    {
        _schedule = new ShGlobalSchedule();
    }
    #endregion


    #region -> Methods
    /// <summary>
    /// Loads the schedule from the JSON file
    /// </summary>
    public void Load()
    {
        // Get the current directory
        string _dir = Directory.GetCurrentDirectory() ??
            throw new Exception("Failed to get the current directory");

        // Prepare the path to the JSON file
        string jsonFilePath = Path.Combine(_dir, "Schedule.json");

        if (File.Exists(jsonFilePath))
        {
            Console.WriteLine($"Reading the schedule from {jsonFilePath}");

            // Read the JSON file
            string jsSch = File.ReadAllText(jsonFilePath)
                ?? throw new FileNotFoundException(jsonFilePath);

            // Deserialize the schedule from the JSON string
            _schedule = JsonSerializer
                .Deserialize<ShGlobalSchedule>(jsSch) ??
                throw new Exception("Failed to deserialize the schedule");
        }
        else
        {
            Console.WriteLine($"Creating an empty schedule");
            _schedule = new ShGlobalSchedule();
        }
    }

    /// <summary>
    /// Shifts the schedule to the specified date
    /// </summary>
    /// <param name="date">The date to shift the schedule to</param>
    public void ShiftToDate(DateTime date)
        => _schedule.ShiftToDate(date);

    /// <summary>
    /// Gets the flights from the specified date and time
    /// </summary>
    /// <param name="time">The date and time to start from</param>
    /// <param name="count">The count of the flights to get</param>
    /// <returns>The list of the flights</returns>
    public IReadOnlyList<ShFlightItem> GetFlights(DateTime time, int count)
    {
        // Check if the day has changed
        if (_day != time.Day)
        {
            _day = time.Day;
            _schedule.ShiftToDate(time);
        }

        return _schedule.GetFlights(Route, time, count);
    }
    #endregion
}
