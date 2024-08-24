namespace RPIDBClock.Svc;

/// <summary>
/// Represents the I2C settings.
/// </summary>
/// <remarks>
/// This class is used to configure 
/// the I2C settings of the application.
/// </remarks>
public class I2CSettings
{
    /// <summary>
    /// Gets or sets the I2C address of the LCD module.
    /// </summary>
    public string LCDAddress { get; set; }

    /// <summary>
    /// Gets or sets the I2C address of the RTC module.
    /// </summary>
    public string RTCAddress { get; set; }
}
