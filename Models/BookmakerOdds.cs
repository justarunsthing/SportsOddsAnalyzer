namespace SportsOddsAnalyzer.Models
{
    public class BookmakerOdds
    {
        public string? BookmakerName { get; set; }
        public DateTime? LastUpdate { get; set; }
        public decimal? HomeWin { get; set; }
        public decimal? Draw { get; set; }
        public decimal? AwayWin { get; set; }
    }
}