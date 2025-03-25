namespace RPIDBClock.Svc.Schedule;

/// <summary>
/// The Schedule Stub
/// </summary>
public class ScheduleStub : IScheduleService
{
    public ShFlightRoute Route { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IReadOnlyList<ShFlightItem> GetFlights(DateTime time, int count) => [];

    public void Load() { }

    public void ShiftToDate(DateTime date) { }
}
