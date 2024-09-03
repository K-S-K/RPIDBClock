using System.Text.Json;

using RPIDBClock.NET.VRN;
using RPIDBClock.Svc.Schedule;

namespace RPIDBClock.Tests;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable xUnit1013 // Public method should be marked as test

public class VRNTests
{
    private JsonSerializerOptions _jso = new() { WriteIndented = true };

    private string _dir = "/Users/ksk-work/Projects/RPI/RPIDBClock/Doc/MyVRN";

    //[Fact]
    [Theory]
    [InlineData("Responce1.json", 4, "S3 09:21 (Karlsruhe Hauptbahnhof) Ludwigshafen, Hauptbahnhof - Heidelberg, Hauptbahnhof")]
    [InlineData("Responce2.json", 4, "S2 16:03 (Kaiserslautern, Hbf) Heidelberg, Hauptbahnhof - Ludwigshafen, Hauptbahnhof")]
    public void VRNResponceParseTest(string fileName, int tripCount, string firstTrip)
    {
        // Path to the JSON file
        // string jsonFilePath = "/Users/ksk-work/Projects/RPI/RPIDBClock/Doc/MyVRN/Responce2.json";
        string jsonFilePath = Path.Combine(_dir, fileName);

        // Read the JSON file
        string jsonString = File.ReadAllText(jsonFilePath);

        List<VRNTrainTrip> trips = VRNResponce.ParseJson(jsonString);

        Assert.Equal(tripCount, trips.Count);
        Assert.Equal(firstTrip, $"{trips[0]}");

        foreach (VRNTrainTrip trip in trips)
        {
            Console.WriteLine();

            Console.WriteLine($"Train: {trip.Train}");
            Console.WriteLine($"Destination: {trip.Destination}");
            Console.WriteLine($"Departure: {trip.DepartureNormal:yyyy.MM.dd HH:mm} ({trip.DepartureExpected:HH:mm})");
            Console.WriteLine($"Arrival:   {trip.ArrivalNormal:yyyy.MM.dd HH:mm} ({trip.ArrivalExpected:HH:mm})");
            Console.WriteLine($"Duration:  {trip.Duration:hh\\:mm}");
            Console.WriteLine($"Start Point: {trip.StartPoint}");
            Console.WriteLine($"End Point: {trip.EndPoint}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Schedule Reading Test
    /// </summary>
    /// <exception cref="FileNotFoundException"></exception>
    [Fact]
    public void ScheduleReadingTest()
    {
        // Prepare the path to the JSON file
        string jsonFilePath = Path.Combine(_dir, "Schedule.json");

        // Read the JSON file
        string jsExpected = File.ReadAllText(jsonFilePath)
            ?? throw new FileNotFoundException(jsonFilePath);

        // Deserialize the schedule from the JSON string
        ShGlobalSchedule schedule = JsonSerializer
            .Deserialize<ShGlobalSchedule>(jsExpected, _jso);

        // Serialize the schedule to the JSON string
        string jsActual = JsonSerializer.Serialize(schedule, _jso);


        // Serialize the secondary schedule to the JSON file
        File.WriteAllText(Path.Combine(_dir, "Schedule2.json"), jsActual);

        // Compare the expected and actual schedules
        Assert.Equal(jsExpected, jsActual);
    }

    /// <summary>
    /// Get Flights Test
    /// </summary>
    [Fact]
    public void GetFlightsTest()
    {
        // Prepare the path to the JSON file
        string jsonFilePath = Path.Combine(_dir, "Schedule.json");

        // Read the JSON file
        string jsExpected = File.ReadAllText(jsonFilePath)
            ?? throw new FileNotFoundException(jsonFilePath);

        // Deserialize the schedule from the JSON string
        ShGlobalSchedule schedule = JsonSerializer
            .Deserialize<ShGlobalSchedule>(jsExpected, _jso);


        // Get the flights from the schedule
        ShFlightRoute route = new()
        {
            Orig = "Ludwigshafen, Hauptbahnhof",
            Dest = "Heidelberg, Hauptbahnhof",
        };

        // The date and time to get the flights for
        DateTime date = new(2024, 9, 3);
        DateTime time = date.Add(new TimeSpan(9, 20, 0));
        DateTime time1 = date.Add(new TimeSpan(9, 21, 0));
        DateTime time2 = date.Add(new TimeSpan(9, 28, 0));

        // Get the flights from the schedule
        // for the date in past (no flights)
        IReadOnlyList<ShFlightItem> flights =
            schedule.GetFlights(route, time, 2);

        // Check the flights
        // There should be no flights
        Assert.Empty(flights);

        // Shift the date 
        // to the particular date flights to be retrieved
        schedule.ShiftToDate(date);

        // Again get the flights from the schedule
        flights =
            schedule.GetFlights(route, time, 2);

        // Check the flights, there should be 2 flights
        Assert.Equal(2, flights.Count);

        // Check the flight time
        Assert.Contains(flights, flight =>
            flight.DepartureExpected == time1);

        // Check the flight time
        Assert.Contains(flights, flight =>
            flight.DepartureExpected == time2);
    }

    /// <summary>
    /// Fill the schedule with the trips from the JSON files
    /// </summary>
    /// <remarks>
    /// The method reads the JSON files with the trips and fills the schedule with them.
    /// Then it checks the schedule for the trips.
    /// </remarks>
    // [Fact]
    public void FillTheScheduleTool()
    {
        // Pathes to the JSON files
        string[] jsonFilePathes = [
            Path.Combine(_dir, "Resp_HD_LU_1.json"),
            Path.Combine(_dir, "Resp_HD_LU_2.json"),
            Path.Combine(_dir, "Resp_HD_LU_3.json"),
            Path.Combine(_dir, "Resp_HD_LU_4.json"),
            Path.Combine(_dir, "Resp_HD_LU_5.json"),
            Path.Combine(_dir, "Resp_HD_LU_6.json"),
            Path.Combine(_dir, "Resp_LU_HD_1.json"),
            Path.Combine(_dir, "Resp_LU_HD_2.json"),
            Path.Combine(_dir, "Resp_LU_HD_3.json"),
            Path.Combine(_dir, "Resp_LU_HD_4.json"),
            Path.Combine(_dir, "Resp_LU_HD_5.json"),
        ];

        // Create the schedule
        ShGlobalSchedule schedule = new();

        // Fill the schedule with the trips from the JSON files
        foreach (string jsonFilePath in jsonFilePathes)
        {
            // Read the particular JSON file
            string jsonString = File.ReadAllText(jsonFilePath);

            // Parse the JSON file
            List<VRNTrainTrip> trips = VRNResponce.ParseJson(jsonString);

            // Add the trips to the schedule
            foreach (VRNTrainTrip trip in trips)
            {
                // Create the route
                ShFlightRoute route = new()
                {
                    Orig = trip.StartPoint,
                    Dest = trip.EndPoint
                };

                // Create the flight
                ShFlightItem flight = new()
                {
                    Name = trip.Train,
                    DepartureExpected = trip.DepartureExpected,
                    ArrivalExpected = trip.ArrivalExpected,
                    DepartureNormal = trip.DepartureNormal,
                    ArrivalNormal = trip.ArrivalNormal,
                    Duration = trip.Duration
                };

                // Add the flight to the schedule
                schedule.AddFlight(route, flight);
            }

            // Check the schedule for the trips
            foreach (VRNTrainTrip trip in trips)
            {
                ShFlightRoute route = new()
                {
                    Orig = trip.StartPoint,
                    Dest = trip.EndPoint
                };

                IReadOnlyList<ShFlightItem> flights =
                    schedule.GetFlights(route, trip.DepartureExpected);

                Assert.Contains(flights, flight =>
                    flight.DepartureExpected == trip.DepartureExpected);
            }
        }

        // Serialize the schedule to the JSON file and read it back
        string jsExpected = JsonSerializer.Serialize(schedule, _jso);
        File.WriteAllText(Path.Combine(_dir, "Schedule.json"), jsExpected);

        // Deserialize the schedule from the JSON file
        schedule = JsonSerializer.Deserialize<ShGlobalSchedule>(jsExpected, _jso);
        string jsActual = JsonSerializer.Serialize(schedule, _jso);

        // Serialize the secondary schedule to the JSON file
        File.WriteAllText(Path.Combine(_dir, "Schedule2.json"), jsActual);

        // Compare the expected and actual schedules
        Assert.Equal(jsExpected, jsActual);
    }
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore xUnit1013 // Public method should be marked as test
#pragma warning restore CS8602 // Dereference of a possibly null reference.
