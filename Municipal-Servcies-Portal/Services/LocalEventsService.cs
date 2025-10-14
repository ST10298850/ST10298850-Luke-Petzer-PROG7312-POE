using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Data;
using Microsoft.EntityFrameworkCore;

namespace Municipal_Servcies_Portal.Services
{
    public class LocalEventsService : ILocalEventsService
    {
        private readonly AppDbContext _context;
        
        // Phase 3 - Data structures for efficient event management
        private SortedDictionary<DateTime, List<Event>> _eventsByDate = new();
        private Dictionary<string, List<Event>> _eventsByCategory = new();
        private HashSet<string> _categories = new();
        private PriorityQueue<Event, DateTime> _upcomingEventsQueue = new();
        private bool _isLoaded = false;

        public LocalEventsService(AppDbContext context)
        {
            _context = context;
        }

        // Phase 3 - Load events into data structures
        private async Task LoadEventsIntoStructuresAsync()
        {
            if (_isLoaded) return;

            var events = await _context.Events.Where(e => e.IsActive).ToListAsync();

            foreach (var ev in events)
            {
                // SortedDictionary by StartDate
                if (!_eventsByDate.ContainsKey(ev.StartDate.Date))
                    _eventsByDate[ev.StartDate.Date] = new List<Event>();
                _eventsByDate[ev.StartDate.Date].Add(ev);

                // Dictionary by Category
                if (!_eventsByCategory.ContainsKey(ev.Category))
                    _eventsByCategory[ev.Category] = new List<Event>();
                _eventsByCategory[ev.Category].Add(ev);

                // HashSet for categories
                _categories.Add(ev.Category);

                // PriorityQueue for upcoming events (future dates only)
                if (ev.StartDate >= DateTime.Now)
                {
                    _upcomingEventsQueue.Enqueue(ev, ev.StartDate);
                }
            }

            _isLoaded = true;
        }

        // Phase 2 - Basic retrieval methods
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Where(e => e.IsActive)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync()
        {
            return await _context.Announcements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.DatePosted)
                .Take(5)
                .ToListAsync();
        }

        // Phase 3 - Data structure based methods
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            await LoadEventsIntoStructuresAsync();
            
            var result = new List<Event>();
            var tempQueue = new PriorityQueue<Event, DateTime>(_upcomingEventsQueue.UnorderedItems);

            while (tempQueue.Count > 0)
            {
                result.Add(tempQueue.Dequeue());
            }
            
            return result;
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(string category)
        {
            await LoadEventsIntoStructuresAsync();
            
            if (_eventsByCategory.TryGetValue(category, out var events))
                return events;
            
            return Enumerable.Empty<Event>();
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            await LoadEventsIntoStructuresAsync();
            return _categories.OrderBy(c => c);
        }

        // Search and filter functionality
        public async Task<IEnumerable<Event>> SearchEventsAsync(string? query, string? category, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                events = events.Where(e => e.Title.Contains(query) || e.Description.Contains(query));

            if (!string.IsNullOrWhiteSpace(category))
                events = events.Where(e => e.Category == category);

            if (startDate.HasValue)
                events = events.Where(e => e.StartDate >= startDate.Value);

            if (endDate.HasValue)
                events = events.Where(e => e.EndDate <= endDate.Value || (!e.EndDate.HasValue && e.StartDate <= endDate.Value));

            events = events.Where(e => e.IsActive);

            return await events.OrderBy(e => e.StartDate).ToListAsync();
        }

        public async Task RecordSearchAsync(string query, string? category, DateTime? startDate, DateTime? endDate)
        {
            // Log search without user info - can be implemented later with a SearchLog table
            await Task.CompletedTask;
        }
    }
}