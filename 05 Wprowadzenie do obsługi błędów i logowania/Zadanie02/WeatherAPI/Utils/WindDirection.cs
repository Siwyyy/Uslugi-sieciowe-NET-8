namespace WeatherAPI.Utils;

public static class WindDirection
{
    public static string GetWindDir(double deg) => deg switch
    {
        >= 337.5 or < 22.5 => "N",
        >= 22.5 and < 67.5 => "NE",
        >= 67.5 and < 112.5 => "E",
        >= 112.5 and < 157.5 => "SE",
        >= 157.5 and < 202.5 => "S",
        >= 202.5 and < 247.5 => "SW",
        >= 247.5 and < 292.5 => "W",
        >= 292.5 and < 337.5 => "NW",
        _ => "?"
    };
}
