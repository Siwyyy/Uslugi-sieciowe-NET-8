using WeatherAPI.Utils;

namespace WeatherAPI.Endpoints;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast/{city}", async (
            string city,
            IHttpClientFactory httpClientFactory,
            IConfiguration config) =>
        {
            var apiKey = config.GetValue<string>("OpenWeatherConfig:ApiKey");
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pl";
            var client = httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return Results.NotFound(new { message = $"Nie znaleziono miasta: {city}" });

                var weatherData = await response.Content.ReadFromJsonAsync<WeatherResponse>();

                if (weatherData == null)
                    return Results.Problem("Data processing error.");

                return Results.Ok(new
                {
                    Miasto = weatherData.City,
                    Opis = weatherData.Description.FirstOrDefault()?.Info,
                    Temperatura = $"{Math.Round(weatherData.Details.Temperature, 1)}°C",
                    Odczuwalna = $"{Math.Round(weatherData.Details.FeelsLike, 1)}°C",
                    Wilgotnosc = $"{weatherData.Details.Humidity}%",
                    Wiatr = new
                    {
                        Predkosc = $"{weatherData.Wind.Speed} m/s",
                        Kierunek = WindDirection.GetWindDir(weatherData.Wind.Deg)
                    }
                });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Server error: {ex.Message}");
            }
        })
        .WithName("GetWeatherByCity")
        .WithOpenApi();

        app.MapGet("/weatherforecast", () => "Free weather forecast by Mikolaj Siwek")
            .WithName("WeatherInfo")
            .WithOpenApi();

        return app;
    }
}