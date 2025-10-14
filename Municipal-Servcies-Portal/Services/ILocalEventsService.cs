using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Services
{
    public interface ILocalEventsService
    {
        // Phase 2 - Basic retrieval methods
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync();
        
        // Phase 3 - Data structure based methods
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetEventsByCategoryAsync(string category);
        Task<IEnumerable<string>> GetCategoriesAsync();
        
        // Search and filter
        Task<IEnumerable<Event>> SearchEventsAsync(string? query, string? category, DateTime? startDate, DateTime? endDate);
        Task RecordSearchAsync(string query, string? category, DateTime? startDate, DateTime? endDate);
    }
}