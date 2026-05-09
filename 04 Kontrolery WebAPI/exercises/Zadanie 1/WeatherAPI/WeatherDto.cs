using System.Text.Json.Serialization;

namespace WeatherAPI
{
    public record WeatherResponse(
        [property: JsonPropertyName("name")] string City,
        [property: JsonPropertyName("main")] MainData Details,
        [property: JsonPropertyName("weather")] WeatherDescription[] Description,
        [property: JsonPropertyName("wind")] WindData Wind
    );

    public record MainData(
        [property: JsonPropertyName("temp")] double Temperature,
        [property: JsonPropertyName("feels_like")] double FeelsLike,
        [property: JsonPropertyName("humidity")] int Humidity
    );

    public record WeatherDescription(
        [property: JsonPropertyName("description")] string Info
    );

    public record WindData(
        [property: JsonPropertyName("speed")] double Speed,
        [property: JsonPropertyName("deg")] int Deg
    );
}