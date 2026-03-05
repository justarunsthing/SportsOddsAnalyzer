using System.Net.Http.Json;
using SportsOddsAnalyzer.Models;
using SportsOddsAnalyzer.Interfaces;

namespace SportsOddsAnalyzer.Providers
{
    public class TheOddsProvider(HttpClient httpClient) : ITheOddsProvider
    {
        private readonly HttpClient _http = httpClient;
        private readonly string _apiKey = "326babd30e8c2f8f6b0c12090e57ec55";

        public async Task<List<Sport>> GetInSeasonSportsAsync()
        {
            string url = $"https://api.the-odds-api.com/v4/sports?apiKey={_apiKey}";

            var response = await _http.GetFromJsonAsync<List<Sport>>(url)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve in season sports");

            return response;
        }

        public async Task<List<Event>> GetEventsByKeyAsync(string sportKey)
        {
            string url = $"https://api.the-odds-api.com/v4/sports/{sportKey}/odds?regions=uk&oddsFormat=decimal&apiKey={_apiKey}";

            var response = await _http.GetFromJsonAsync<List<Event>>(url)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, $"Failed to retrieve events for sport key: {sportKey}");

            return response;
        }

        public async Task<Event> GetEventByIdAsync(string sportKey, string id)
        {
            string url = $"https://api.the-odds-api.com/v4/sports/{sportKey}/events/{id}/odds?apiKey={_apiKey}&regions=uk&markets=h2h";

            var response = await _http.GetFromJsonAsync<Event>(url)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, $"Failed to retrieve event for id: {id}, sport key: {sportKey}");

            return response;
        }
    }
}