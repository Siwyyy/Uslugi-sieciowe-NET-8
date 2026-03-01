using WeatherAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

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

        if (!response.IsSuccessStatusCode) return Results.NotFound(new { message = $"Nie znaleziono miasta: {city}" });


        var weatherData = await response.Content.ReadFromJsonAsync<WeatherResponse>();

        if (weatherData == null) return Results.Problem("Data processing error.");

        // 4. Zwrócenie przefiltrowanego wyniku
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
                Kierunek = GetWindDir(weatherData.Wind.Deg)
            }
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Server error: {ex.Message}");
    }
});

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () => "Free weather forecast by Miko³aj Siwek");

app.Run();
return;

string GetWindDir(double deg) => deg switch
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