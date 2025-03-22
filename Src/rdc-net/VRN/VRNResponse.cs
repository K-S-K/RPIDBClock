using System.Text.Json.Nodes;

namespace RPIDBClock.NET.VRN;

public class VRNResponse
{
    private const string _format = "dd.MM.yyyy HH:mm";

    public static List<VRNTrainTrip> ParseJson(string jsonString)
    {
        List<VRNTrainTrip> records = [];

        // Parse the JSON
        JsonNode? jsonNode = JsonNode.Parse(jsonString);

        // Extract the trips data
        JsonArray? trips = jsonNode?["trips"]?.AsArray();

        if (trips == null)
        {
            Console.WriteLine("No trips found");
            return records;
        }

        // Enumerate trips
        foreach (JsonNode? trip in trips)
        {
            JsonNode? leg = trip?["legs"]?[0];
            JsonNode? startPoint = leg?["points"]?[0];
            JsonNode? endPoint = leg?["points"]?[1];

            if (leg == null)
            {
                Console.WriteLine("No legs");
                continue;
            }

            VRNTrainTrip record = new()
            {
                Train = $"{leg?["mode"]?["number"]}",
                Destination = $"{leg?["mode"]?["destination"]}",
                DepartureNormal = DateTime.ParseExact($"{startPoint?["dateTime"]?["date"]} {startPoint?["dateTime"]?["time"]}", _format, null),
                DepartureExpected = DateTime.ParseExact($"{startPoint?["dateTime"]?["rtDate"]} {startPoint?["dateTime"]?["rtTime"]}", _format, null),
                ArrivalNormal = DateTime.ParseExact($"{endPoint?["dateTime"]?["date"]} {endPoint?["dateTime"]?["time"]}", _format, null),
                ArrivalExpected = DateTime.ParseExact($"{endPoint?["dateTime"]?["date"]} {endPoint?["dateTime"]?["time"]}", _format, null),
                Duration = TimeSpan.Parse($"{trip?["duration"]}"),
                StartPoint = $"{startPoint?["name"]}",
                EndPoint = $"{endPoint?["name"]}"
            };

            records.Add(record);
        }
        return records;
    }
}
