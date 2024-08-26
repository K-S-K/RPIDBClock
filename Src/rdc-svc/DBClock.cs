using RPIDBClock.LCD;
using RPIDBClock.NET;
using RPIDBClock.RTC;

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
    private void PrepareDisplay()
    {
        // Create a custom character for the degree symbol (°).
        byte[] degSymbol =
        [
            0b00110,
            0b01001,
            0b01001,
            0b00110,
            0b00000,
            0b00000,
            0b00000,
            0b00000
        ];
        lcd.CreateCustomCharacter(0, degSymbol);

        // Create a custom character for the clock symbol (🕒).
        byte[] clockSymbol =
        [
            0b00000,
            0b01110,
            0b10101,
            0b10111,
            0b10001,
            0b01110,
            0b00000,
            0b00000
        ];
        lcd.CreateCustomCharacter(1, clockSymbol);

        // Create a custom character for the temperature symbol (🌡).
        byte[] tempSymbol =
        [
            0b00100,
            0b01010,
            0b01010,
            0b01110,
            0b01110,
            0b11111,
            0b11111,
            0b01110
        ];
        lcd.CreateCustomCharacter(2, tempSymbol);

        // Create a custom character for the humidity symbol (💧).
        byte[] humSymbol =
        [
            0b00000,
            0b00100,
            0b00100,
            0b01110,
            0b10101,
            0b10101,
            0b10001,
            0b01110
        ];
        lcd.CreateCustomCharacter(3, humSymbol);

        // Create a custom character for the earth symbol (🌍).
        byte[] earthSymbol =
        [
            0b00000,
            0b01110,
            0b11111,
            0b11111,
            0b01110,
            0b10101,
            0b11111,
            0b00000
        ];
        lcd.CreateCustomCharacter(4, earthSymbol);


        // Prepare the LCD display.
        lcd.Clear();
        lcd.Write(1, 18, "\x00");  // Display the custom character (°)
        lcd.Write(1, 19, "C");  // Display the temperature unit (C)

        // Display the custom character (🕒) for the clock symbol.
        lcd.Write(0, 0, "\x01");

        // Display the custom character (🌡) for the temperature symbol.
        lcd.Write(1, 0, "\x02");

        // Display the custom character (🌍) for the earth symbol
        lcd.Write(2, 0, "\x04");
    }

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
