/// <summary>
/// ChellGPT be blessed your soul
/// </summary>
public class TimeSimulator
{
    private DateTime _currentTime;
    private const int TicksPerHour = 1;
    private const int NightStartHour = 20;
    private const int NightEndHour = 6;
    private const string DayEmoji = "\U0001F31E"; // sun emoji
    private const string NightEmoji = "\U0001F319"; // crescent moon emoji

    public TimeSimulator()
    {
        _currentTime = new DateTime(638175612000000000);
    }

    public void Tick()
    {
        _currentTime = _currentTime.AddHours(TicksPerHour);
    }

    public string Display()
    {
        var currentHour = _currentTime.Hour;
        var dayNightCycle = currentHour is >= NightStartHour or < NightEndHour ? NightEmoji : DayEmoji;

        return _currentTime.ToString("MMMM dd HH:mm") + " " + dayNightCycle;
    }
}