using System;

namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// The Schedule Service
/// </summary>
public interface IScheduleService
{
    /// <summary>
    /// The flight route
    /// </summary>
    ShFlightRoute Route { get; set; }


    /// <summary>
    /// Loads the schedule from the JSON file
    /// </summary>
    void Load();

    /// <summary>
    /// Shifts the schedule to the specified date
    /// </summary>
    /// <param name="date">The date to shift the schedule to</param>
    void ShiftToDate(DateTime date);

    /// <summary>
    /// Gets the flights from the specified date and time
    /// </summary>
    /// <param name="time">The date and time to start from</param>
    /// <param name="count">The count of the flights to get</param>
    /// <returns>The list of the flights</returns>
    IReadOnlyList<ShFlightItem> GetFlights(DateTime time, int count);
}
