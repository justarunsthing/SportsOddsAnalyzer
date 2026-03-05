namespace SportsOddsAnalyzer.Models
{
    public class AnalyzedEvent : Event
    {
        public List<BookmakerOdds> FlatBookmakers { get; set; } = [];
        public BestOddsSummary BestOdds { get; set; } = new();
        public MarginInfo Margin { get; set; } = new();
        public ConsensusOdds Consensus { get; set; } = new();
        public ValueBetsInfo ValueBets { get; set; } = new();
    }

    public class BestOddsSummary
    {
        public OddsInfo Home { get; set; } = new();
        public OddsInfo Draw { get; set; } = new();
        public OddsInfo Away { get; set; } = new();
    }

    public class OddsInfo
    {
        public decimal? Odds { get; set; }
        public string? Bookmaker { get; set; }
    }

    public class MarginInfo
    {
        public decimal Percent { get; set; }
        public string? Description { get; set; }
    }

    public class ConsensusOdds
    {
        public decimal Home { get; set; }
        public decimal Draw { get; set; }
        public decimal Away { get; set; }
    }

    public class ValueBetsInfo
    {
        public List<string> Home { get; set; } = [];
        public List<string> Draw { get; set; } = [];
        public List<string> Away { get; set; } = [];
        public bool HasAny => Home.Count != 0 || Draw.Count != 0 || Away.Count != 0;
    }
}