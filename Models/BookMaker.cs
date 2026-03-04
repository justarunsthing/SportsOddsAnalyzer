using System.Text.Json.Serialization;

namespace SportsOddsAnalyzer.Models
{
    public class BookMaker
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("markets")]
        public List<Market> Markets { get; set; } = [];

    }
}