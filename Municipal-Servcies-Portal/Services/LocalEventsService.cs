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
        
        /// <summary>
        /// Searches for events based on multiple filter criteria
        /// This method queries the database and applies filters sequentially
        /// NOTE: EF Core requires case-insensitive search to use EF.Functions.Like() instead of Contains with OrdinalIgnoreCase
        /// </summary>
        /// <param name="name">Event name/title to search for (partial match, case-insensitive)</param>
        /// <param name="category">Exact category match (case-insensitive)</param>
        /// <param name="startDate">Filter events starting on or after this date</param>
        /// <param name="endDate">Filter events starting on or before this date</param>
        /// <returns>List of events matching all provided criteria</returns>
        public async Task<IEnumerable<Event>> SearchEventsAsync(string? name, string? category, DateTime? startDate, DateTime? endDate)
        {
            // Start with a queryable collection from the database
            // AsQueryable() allows us to build the query before executing it
            var query = _context.Events.AsQueryable();

            // Apply name filter if provided
            // Use EF.Functions.Like() for case-insensitive search that translates to SQL
            // The % wildcard means "match any characters before or after"
            if (!string.IsNullOrEmpty(name))
            {
                // Convert to lowercase for case-insensitive search that works with SQL
                query = query.Where(e => e.Title.ToLower().Contains(name.ToLower()));
            }

            // Apply category filter if provided
            // Use ToLower() for case-insensitive comparison that EF Core can translate
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category.ToLower() == category.ToLower());
            }

            // Apply start date filter if provided
            // This filters events that start on or after the specified date
            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= startDate.Value);
            }

            // Apply end date filter if provided
            // This filters events that start on or before the specified date
            if (endDate.HasValue)
            {
                query = query.Where(e => e.StartDate <= endDate.Value);
            }

            // Only show active events
            query = query.Where(e => e.IsActive);

            // Execute the query and return the results as a list
            // ToListAsync() actually runs the database query
            return await query.OrderBy(e => e.StartDate).ToListAsync();
        }


        public async Task RecordSearchAsync(string query, string? category, DateTime? startDate, DateTime? endDate)
        {
            // Log search without user info - can be implemented later with a SearchLog table
            await Task.CompletedTask;
        }
    }
}