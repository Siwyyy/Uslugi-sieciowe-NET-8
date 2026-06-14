namespace WeatherAPI.Utils;

public class IdGenerator
{
    private int _value;

    public int Next() => Interlocked.Increment(ref _value);
}
