using System.Text.Json.Serialization;

namespace SportsOddsAnalyzer.Models
{
    public class Event
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("sport_key")]
        public string? SportKey { get; set; }

        [JsonPropertyName("sport_title")]
        public string? SportTitle { get; set; }

        [JsonPropertyName("commence_time")]
        public DateTime? CommenceTime { get; set; }

        [JsonPropertyName("home_team")]
        public string? HomeTeam { get; set; }

        [JsonPropertyName("away_team")]
        public string? AwayTeam { get; set; }
        public List<BookMaker> BookMakers { get; set; } = [];
    }
}