using SportsOddsAnalyzer.Models;

namespace SportsOddsAnalyzer.Interfaces
{
    public interface ITheOddsProvider
    {
        Task<List<Sport>> GetInSeasonSportsAsync();
        Task<List<Event>> GetEventsByKeyAsync(string sportKey);
        Task<Event> GetEventByIdAsync(string sportKey, string id);
    }
}