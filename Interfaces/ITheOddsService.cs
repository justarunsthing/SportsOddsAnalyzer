using SportsOddsAnalyzer.Models;

namespace SportsOddsAnalyzer.Interfaces
{
    public interface ITheOddsService
    {
        Task<List<Sport>> GetInSeasonSportsAsync();
        Task<List<Event>> GetEventsByKeyAsync(string sportKey);
        Task<AnalyzedEvent> GetAnalyzedEventAsync(string sportKey, string eventId);
    }
}