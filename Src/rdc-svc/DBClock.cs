using RPIDBClock.CLK;
using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;
using RPIDBClock.Svc.Schedule;

namespace RPIDBClock.Svc;

public class DBClock : IDBClock
{
    #region -> Fields
    /// <summary>
    /// The NTP client service.
    /// </summary>
    private readonly INTPService ntp;

    /// <summary>
    /// The LCD display service.
    /// </summary>
    private readonly ILCDService lcd;

    /// <summary>
    /// The RTC service.
    /// </summary>
    private readonly IRTCService rtc;

    /// <summary>
    /// The timer service.
    /// </summary>
    private readonly ITimerService tmr;

    /// <summary>
    /// The global schedule.
    /// </summary>
    private readonly IScheduleService sch;
    #endregion


    #region -> Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="DBClock"/> class.
    /// </summary>
    /// <param name="ntp">The NTP client service.</param>
    /// <param name="lcd">The LCD display service.</param>
    /// <param name="rtc">The RTC service.</param>
    /// <remarks>
    /// This constructor initializes the NTP client, the LCD display, and the RTC service.
    /// It also prepares the LCD display and the timer for the clock update.
    /// </remarks>
    public DBClock(ITimerService tmr, INTPService ntp, ILCDService lcd, IRTCService rtc, IScheduleService sch)
    {
        this.tmr = tmr;
        this.ntp = ntp;
        this.lcd = lcd;
        this.rtc = rtc;
        this.sch = sch;

        // Subscribe to the timer event to update the clock.
        tmr.TimerEvent += (sender, args) => OnTimer(args);

        // Prepare the LCD display.
        lcd.PrepareDisplay();
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Handles the timer event.
    /// </summary>
    /// <param name="args">The event arguments.</param>
    /// <remarks>
    /// This method reads the current time and temperature 
    /// from the RTC module and displays them on the LCD display.
    /// </remarks>
    private void OnTimer(TimerEventArgs args)
    {
        // Display the current time and temperature on the LCD display.
        DateTime time = args.Time.ToLocalTime();

        IReadOnlyList<ShFlightItem> flights = sch.GetFlights(time, 2);

        double temp = double.Round(rtc.ReadTemperature(), 1, MidpointRounding.ToEven);

        TimeZoneInfo.ClearCachedData();
        var tz = TimeZoneInfo.Local;

        lcd.Write(0, 1, $"{time:yyyy.MM.dd HH:mm:ss}");

        if (time.Second % 10 == 0)
        {
            lcd.Write(1, 0, "\x02");
            lcd.Write(1, 1, $"Temperature: {temp:F1} C");
            lcd.Write(1, 18, "\x00");
        }
        else
        {
            //lcd.Write(1, 0, $" LU Hbf -> HD Hbf   ");
            lcd.Write(1, 0, $" Ludw.Hbf - Heid.Hbf");
        }


        //*
        if (flights.Count > 0)
        {
            lcd.Write(2, 1, $"{flights[0]}");
        }

        if (flights.Count > 1)
        {
            lcd.Write(3, 1, $"{flights[1]}");
        }
        if (flights.Count == 0)
        {
            lcd.Write(2, 1, $"{tz.Id,-19}");
        }
        //*/

        Console.WriteLine($"Time: {time:yyyy.MM.dd HH:mm:ss}  Temperature: {rtc.ReadTemperature()}°C");
    }
    #endregion


    #region -> Methods
    /// <summary>
    /// Loads the schedule and  other preparations.
    /// </summary>
    public void Prepare()
    {
        // Load the schedule from the JSON file.
        sch.Load();

        // Set the route for the schedule.
        sch.Route = new()
        {
            Orig = "Ludwigshafen, Hauptbahnhof",
            Dest = "Heidelberg, Hauptbahnhof",
        };
    }

    /// <summary>
    /// Starts the clock.
    /// </summary>
    /// <remarks>
    /// This method gets the current network time and sets it on the RTC module.
    /// It also resumes the timer.
    /// </remarks>
    public void Start()
    {
        // Get the current network time and set it on the RTC module.
        DateTime networkTime = ntp.GetNetworkTime();
        rtc.WriteTime(networkTime);

        Resume();
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause() => tmr.Pause();

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void Resume() => tmr.Resume();

    /// <summary>
    /// Releases all resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        tmr?.Dispose();
    }
    #endregion
}
