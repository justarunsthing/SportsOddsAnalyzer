using System.Text.Json.Serialization;

namespace SportsOddsAnalyzer.Models
{
    public class Outcome
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }
}