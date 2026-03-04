using System.Net.Http.Json;
using SportsOddsAnalyzer.Models;

namespace SportsOddsAnalyzer.Services
{
    public class TheOddsService(HttpClient httpClient, IConfiguration config)
    {
        private readonly HttpClient _http = httpClient;
        private readonly string? _apiKey = config["TheOddsKey"];

        public async Task<List<Sport>> GetInSeasonSportsAsync()
        {
            string url = $"https://api.the-odds-api.com/v4/sports?apiKey=326babd30e8c2f8f6b0c12090e57ec55";

            var response = await _http.GetFromJsonAsync<List<Sport>>(url)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve in season sports");

            return response;
        }
    }
}