using RPIDBClock.NET.VRN;

namespace RPIDBClock.Tests;

public class VRNTests
{
    //[Fact]
    [Theory]
    [InlineData("Responce1.json", 4, "S3 09:21 (Karlsruhe Hauptbahnhof) Ludwigshafen, Hauptbahnhof - Heidelberg, Hauptbahnhof")]
    [InlineData("Responce2.json", 4, "S2 16:03 (Kaiserslautern, Hbf) Heidelberg, Hauptbahnhof - Ludwigshafen, Hauptbahnhof")]
    public void VRNResponceParseTest(string fileName, int tripCount, string firstTrip)
    {
        // Path to the JSON file
        // string jsonFilePath = "/Users/ksk-work/Projects/RPI/RPIDBClock/Doc/MyVRN/Responce2.json";
        string jsonFilePath = Path.Combine("/Users/ksk-work/Projects/RPI/RPIDBClock/Doc/MyVRN", fileName);

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
}
