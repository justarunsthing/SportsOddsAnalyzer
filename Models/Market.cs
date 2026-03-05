using System.Text.Json.Serialization;

namespace SportsOddsAnalyzer.Models
{
    public class Market
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("last_update")]
        public DateTime? LastUpdate { get; set; }

        [JsonPropertyName("outcomes")]
        public List<Outcome> Outcomes { get; set; } = [];
    }
}