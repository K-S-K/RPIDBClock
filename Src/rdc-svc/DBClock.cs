using RPIDBClock.LCD;
using RPIDBClock.RTC;
using RPIDBClock.NET.NTP;

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
    /// The timer that triggers the clock update.
    /// </summary>
    private Timer? timer;
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
    public DBClock(INTPService ntp, ILCDService lcd, IRTCService rtc)
    {
        this.ntp = ntp;
        this.lcd = lcd;
        this.rtc = rtc;

        PrepareDisplay();
        PrepareTimer();
    }
    #endregion


    #region -> Implementation
    /// <summary>
    /// Prepares the timer.
    /// </summary>
    private void PrepareTimer()
    {
        // Define the callback method for the timer.
        System.Threading.TimerCallback callback = (state) =>
        {
            DateTime time = rtc.ReadTime();

            OnTimer(new TimerEventArgs(time));
        };

        // Create a new timer that triggers the callback method every second.
        timer = new Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
    }

    /// <summary>
    /// Prepares the LCD display.
    /// </summary>
    /// <remarks>
    /// This method creates custom characters for the degree symbol (°), the clock symbol (🕒),
    /// the temperature symbol (🌡), and the humidity symbol (💧).
    /// </remarks>
    private void PrepareDisplay() => lcd.PrepareDisplay();

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
        double temp = double.Round(rtc.ReadTemperature(), 1, MidpointRounding.ToEven);

        TimeZoneInfo.ClearCachedData();
        var tz = TimeZoneInfo.Local;

        lcd.Write(0, 1, $"{time:yyyy.MM.dd HH:mm:ss}");
        lcd.Write(1, 1, $"Temperature: {temp:F1}");
        lcd.Write(2, 1, $"{tz.Id,-19}");

        Console.WriteLine($"Time: {time:yyyy.MM.dd HH:mm:ss}  Temperature: {rtc.ReadTemperature()}°C");
    }
    #endregion


    #region -> Methods
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
    public void Pause() => timer?.Change(Timeout.Infinite, Timeout.Infinite);

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void Resume() => timer?.Change(0, 1000);

    /// <summary>
    /// Releases all resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        timer?.Dispose();
    }
    #endregion
}
