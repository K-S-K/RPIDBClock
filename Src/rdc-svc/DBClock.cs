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

        ntp.LogEvent += (sender, msg) => OnLogEvent(msg);
        tmr.LogEvent += (sender, msg) => OnLogEvent(msg);
        // TODO: lcd.LogEvent += (sender, msg) => OnLogEvent(msg);
        // TODO: rtc.LogEvent += (sender, msg) => OnLogEvent(msg);
        // TODO: sch.LogEvent += (sender, msg) => OnLogEvent(msg);
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

        lcd.Write(0, 0, "\x01");
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

    /// <summary>
    /// Handles the log event.
    /// </summary>
    /// <param name="msg">The log message.</param>
    private void OnLogEvent(string msg)
    {
        // Cut the message to 20 characters.
        msg = msg.Length > 20 ? msg[..20] : msg;

        // Display the message on the LCD display.
        lcd.Write(3, 0, msg.PadRight(20));
        Thread.Sleep(2000);
        lcd.Write(3, 0, " ".PadRight(20));
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
        // Get, if possible, the current network time and set it on the RTC module.
        // Than, set the RTC time to the current system time.
        SyncSystemTime(true);

        Resume();
    }

    /// <summary>
    /// Synchronizes the RTC time 
    // and the system time with 
    // the network time.
    /// </summary>
    /// <param name="withNetworkTime">Indicates 
    // whether to synchronize the system time 
    // with the network time.</param>
    public void SyncSystemTime(bool withNetworkTime)
    {
        lcd.Clear();

        lcd.Write(0, 0, $"Sync Sys Time ({(withNetworkTime ? "NTP" : "RTC")})");

        if (withNetworkTime)
        {
            // Get the current network time and set it on the RTC module.
            lcd.Write(1, 0, "Get Network Time...");
            if (ntp.GetNetworkTime(out DateTime networkTime))
            {
                // We must set time it as soon as possible, 
                // because time is running.
                lcd.Write(3, 0, "Set RTC Time...");
                rtc.WriteTime(networkTime);
                lcd.Write(2, 0, $"{networkTime:yyyy.MM.dd HH:mm:ss}");
                Thread.Sleep(3000);
            }
            else
            {
                lcd.Write(2, 0, "NTP Error");
                Thread.Sleep(3000);
            }

            // Clear the bottom lines of the LCD display.
            lcd.Write(1, 0, " ".PadRight(20));
            lcd.Write(2, 0, " ".PadRight(20));
            lcd.Write(3, 0, " ".PadRight(20));
        }

        lcd.Write(1, 0, "Get RTC Time...");
        DateTime dt = rtc.ReadTime();

        lcd.Write(3, 0, "Set System Time...");
        tmr.SetSystemTime(dt);
        lcd.Write(2, 0, $"{dt:yyyy.MM.dd HH:mm:ss}");

        Thread.Sleep(5000);

        lcd.Clear();
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
