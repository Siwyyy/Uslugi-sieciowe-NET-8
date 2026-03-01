var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () => "WeatherAPI\n" +
                                     "/temperature\n" +
                                     "/wind_direction");

var directions = new[]
{
    "N", "NE", "E", "SE", "S", "SW", "W", "NW"
};

app.MapGet("/temperature", () => Random.Shared.Next(-21, 37));
app.MapGet("/wind_direction", () => directions[Random.Shared.Next(0, directions.Length)]);

app.Run();