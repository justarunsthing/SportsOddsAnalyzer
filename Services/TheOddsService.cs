using SportsOddsAnalyzer.Models;
using SportsOddsAnalyzer.Interfaces;

namespace SportsOddsAnalyzer.Services
{
    public class TheOddsService(ITheOddsProvider provider) : ITheOddsService
    {
        private readonly ITheOddsProvider _provider = provider;

        public async Task<List<Sport>> GetInSeasonSportsAsync()
        {
            return await _provider.GetInSeasonSportsAsync();
        }

        public async Task<List<Event>> GetEventsByKeyAsync(string sportKey)
        {
            return await _provider.GetEventsByKeyAsync(sportKey);
        }

        public async Task<AnalyzedEvent> GetAnalyzedEventAsync(string sportKey, string eventId)
        {
            var rawEvent = await _provider.GetEventByIdAsync(sportKey, eventId);

            if (rawEvent == null)
            {
                return new AnalyzedEvent();
            }

            var analyzedEvent = new AnalyzedEvent
            {
                Id = rawEvent.Id,
                SportKey = rawEvent.SportKey,
                SportTitle = rawEvent.SportTitle,
                CommenceTime = rawEvent.CommenceTime,
                HomeTeam = rawEvent.HomeTeam,
                AwayTeam = rawEvent.AwayTeam
            };

            var flatBookmakers = new List<BookmakerOdds>();

            foreach (var bookmaker in rawEvent.Bookmakers ?? Enumerable.Empty<BookMaker>())
            {
                var h2hMarket = bookmaker.Markets?.FirstOrDefault(m => string.Equals(m.Key, "h2h", StringComparison.OrdinalIgnoreCase));

                if (h2hMarket == null)
                {
                    continue;
                }

                var outcomes = h2hMarket.Outcomes ?? Enumerable.Empty<Outcome>();

                var homeOutcome = outcomes.FirstOrDefault(o => string.Equals(o.Name, rawEvent.HomeTeam, StringComparison.OrdinalIgnoreCase));
                var drawOutcome = outcomes.FirstOrDefault(o => string.Equals(o.Name, "Draw", StringComparison.OrdinalIgnoreCase));
                var awayOutcome = outcomes.FirstOrDefault(o => string.Equals(o.Name, rawEvent.AwayTeam, StringComparison.OrdinalIgnoreCase));

                flatBookmakers.Add(new BookmakerOdds
                {
                    BookmakerName = bookmaker.Title ?? bookmaker.Key ?? "Unknown",
                    LastUpdate = h2hMarket.LastUpdate,
                    HomeWin = homeOutcome?.Price,
                    Draw = drawOutcome?.Price,
                    AwayWin = awayOutcome?.Price
                });
            }

            analyzedEvent.FlatBookmakers = flatBookmakers;

            if (flatBookmakers.Count == 0)
            {
                return analyzedEvent;
            }

            // 1. Best Odds
            analyzedEvent.BestOdds = new BestOddsSummary
            {
                Home = GetBestOdds(flatBookmakers, b => b.HomeWin),
                Draw = GetBestOdds(flatBookmakers, b => b.Draw),
                Away = GetBestOdds(flatBookmakers, b => b.AwayWin)
            };

            // 2. Margin/Overround
            analyzedEvent.Margin = ComputeMargin(flatBookmakers);

            // 3. Average
            analyzedEvent.Consensus = new ConsensusOdds
            {
                Home = flatBookmakers.Where(b => b.HomeWin.HasValue).Average(b => b.HomeWin!.Value),
                Draw = flatBookmakers.Where(b => b.Draw.HasValue).Average(b => b.Draw!.Value),
                Away = flatBookmakers.Where(b => b.AwayWin.HasValue).Average(b => b.AwayWin!.Value)
            };

            // 4. Value Bets
            analyzedEvent.ValueBets = ComputeValueBets(flatBookmakers, analyzedEvent.Consensus);

            return analyzedEvent;
        }

        private static OddsInfo GetBestOdds(List<BookmakerOdds> bookmakers, Func<BookmakerOdds, decimal?> selector)
        {
            var best = bookmakers
                .Where(b => selector(b).HasValue)
                .MinBy(b => selector(b)!.Value);

            return new OddsInfo
            {
                Odds = selector(best),
                Bookmaker = best?.BookmakerName
            };
        }

        private static MarginInfo ComputeMargin(List<BookmakerOdds> bookmakers)
        {
            var valid = bookmakers.Where(b => b.HomeWin.HasValue && b.Draw.HasValue && b.AwayWin.HasValue).ToList();

            if (valid.Count == 0)
            {
                return new MarginInfo();
            }

            var overrounds = valid.Select(b => (1 / b.HomeWin!.Value) + (1 / b.Draw!.Value) + (1 / b.AwayWin!.Value));
            var avgOverround = overrounds.Average();
            var margin = (avgOverround - 1) * 100;

            return new MarginInfo
            {
                Percent = margin,
                Description = margin < 6 ? "Low (good for bettors)" : margin > 10 ? "High" : "Typical"
            };
        }

        private static ValueBetsInfo ComputeValueBets(List<BookmakerOdds> bookmakers, ConsensusOdds consensus)
        {
            var value = new ValueBetsInfo
            {
                Home = bookmakers
                    .Where(b => b.HomeWin.HasValue && b.HomeWin.Value < consensus.Home * 0.95m)
                    .Select(b => b.BookmakerName ?? "Unknown")
                    .ToList(),

                Draw = bookmakers
                    .Where(b => b.Draw.HasValue && b.Draw.Value < consensus.Draw * 0.95m)
                    .Select(b => b.BookmakerName ?? "Unknown")
                    .ToList(),

                Away = bookmakers
                    .Where(b => b.AwayWin.HasValue && b.AwayWin.Value < consensus.Away * 0.95m)
                    .Select(b => b.BookmakerName ?? "Unknown")
                    .ToList()
            };

            return value;
        }
    }
}