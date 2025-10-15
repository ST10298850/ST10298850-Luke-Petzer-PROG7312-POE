using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Data;
using Microsoft.EntityFrameworkCore;

namespace Municipal_Servcies_Portal.Services
{
    public class LocalEventsService : ILocalEventsService
    {
        private readonly AppDbContext _context;
        private readonly SearchHistoryService _searchHistoryService;
        
        // Data structures for efficient event management
        private SortedDictionary<DateTime, List<Event>> _eventsByDate = new();
        private Dictionary<string, List<Event>> _eventsByCategory = new();
        private HashSet<string> _categories = new();
        private PriorityQueue<Event, DateTime> _upcomingEventsQueue = new();
        private HashSet<DateTime> _uniqueEventDates = new();
        private Stack<Event> _recentlyViewedEvents = new();

        public LocalEventsService(AppDbContext context, SearchHistoryService searchHistoryService)
        {
            _context = context;
            _searchHistoryService = searchHistoryService;
        }

        // Load events into data structures
        private async Task LoadEventsIntoStructuresAsync()
        {
            // Always reload to ensure fresh data for recommendations
            // Clear existing data structures
            _eventsByDate.Clear();
            _eventsByCategory.Clear();
            _categories.Clear();
            _upcomingEventsQueue.Clear();
            _uniqueEventDates.Clear();

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

                // HashSet for unique event dates
                _uniqueEventDates.Add(ev.StartDate.Date);

                // PriorityQueue for upcoming events (future dates only)
                if (ev.StartDate >= DateTime.Now)
                {
                    _upcomingEventsQueue.Enqueue(ev, ev.StartDate);
                }
            }
        }

        //Basic retrieval methods
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
                .Take(3)
                .ToListAsync();
        }

        // Data structure based methods
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

        // Public method to get unique event dates
        public async Task<IEnumerable<DateTime>> GetUniqueEventDatesAsync()
        {
            await LoadEventsIntoStructuresAsync();
            return _uniqueEventDates;
        }

        // Public method to get recently viewed events (top N)
        public IEnumerable<Event> GetRecentlyViewedEvents(int count = 5)
        {
            return _recentlyViewedEvents.Take(count);
        }

        // Call this when an event is viewed
        public void RecordEventViewed(Event ev)
        {
            _recentlyViewedEvents.Push(ev);
            // Optionally limit stack size
            if (_recentlyViewedEvents.Count > 20)
            {
                var tempStack = new Stack<Event>(_recentlyViewedEvents.Reverse().Skip(1));
                _recentlyViewedEvents = tempStack;
            }
        }

        // Search and filter functionality
        
        /// <summary>
        /// Searches for events based on multiple filter criteria
        /// This method queries the database and applies filters sequentially
        /// </summary>
        /// <param name="name">Event name/title to search for (partial match, case-insensitive)</param>
        /// <param name="category">Exact category match (case-insensitive)</param>
        /// <param name="startDate">Filter events starting on or after this date</param>
        /// <param name="endDate">Filter events starting on or before this date (optional)</param>
        /// <returns>List of events matching all provided criteria</returns>
        public async Task<IEnumerable<Event>> SearchEventsAsync(string? name, string? category, DateTime? startDate, DateTime? endDate)
        {
            // Start with a queryable collection from the database
            var query = _context.Events.AsQueryable();

            // Apply name filter if provided
            if (!string.IsNullOrEmpty(name))
            {
                // Convert to lowercase for case-insensitive search that works with SQL
                query = query.Where(e => e.Title.ToLower().Contains(name.ToLower()));
            }

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category.ToLower() == category.ToLower());
            }

            // Show events FROM startDate onwards (ignore time component)
            if (startDate.HasValue)
            {
                var dateOnly = startDate.Value.Date;
                query = query.Where(e => e.StartDate.Date >= dateOnly);
            }

            // Only apply endDate if provided (currently null from controller for open-ended date range)
            if (endDate.HasValue)
            {
                var dateOnly = endDate.Value.Date;
                query = query.Where(e => e.StartDate.Date <= dateOnly);
            }

            // Only show active events
            query = query.Where(e => e.IsActive);

            // Execute the query and return the results ordered by date
            return await query.OrderBy(e => e.StartDate).ToListAsync();
        }


        // Record a user search for recommendations
        public Task RecordSearchAsync(string? searchName, string? category, DateTime? date, DateTime? endDate)
        {
            // Track all search parameters including text searches
            _searchHistoryService.AddSearch(searchName, category, date);
            return Task.CompletedTask;
        }

        // Recommend events based on most frequent category/date/search terms in search history
        public async Task<IEnumerable<Event>> GetRecommendedEventsAsync()
        {
            await LoadEventsIntoStructuresAsync();
            var searchHistory = _searchHistoryService.GetSearchHistory();

            if (searchHistory.Count == 0)
            {
                // Fallback: recommend upcoming events
                return _upcomingEventsQueue.UnorderedItems
                    .Select(x => x.Element)
                    .OrderBy(e => e.StartDate)
                    .Take(3)
                    .ToList();
            }

            // Analyze search history including search text
            var categoryCounts = new Dictionary<string, int>();
            var searchTerms = new List<string>();

            foreach (var item in searchHistory)
            {
                // Track categories
                if (!string.IsNullOrEmpty(item.Category))
                {
                    if (!categoryCounts.ContainsKey(item.Category))
                        categoryCounts[item.Category] = 0;
                    categoryCounts[item.Category]++;
                }

                // Track search terms for keyword-based recommendations
                if (!string.IsNullOrEmpty(item.SearchText))
                {
                    searchTerms.Add(item.SearchText.ToLower());
                }
            }
            
            // Get most frequent category
            var topCategory = categoryCounts.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            // Build recommendations
            var recommended = new List<Event>();
            
            // Strategy 1: Recommend based on most searched category
            if (!string.IsNullOrEmpty(topCategory) && _eventsByCategory.ContainsKey(topCategory))
            {
                recommended.AddRange(_eventsByCategory[topCategory]
                    .Where(e => e.StartDate >= DateTime.Now)
                    .OrderBy(e => e.StartDate)
                    .Take(3));
            }
            
            // Strategy 2: Recommend events matching recent search terms
            if (recommended.Count < 3 && searchTerms.Any())
            {
                var existingIds = recommended.Select(e => e.Id).ToHashSet();
                var keywordMatches = await _context.Events
                    .Where(e => e.IsActive && e.StartDate >= DateTime.Now && !existingIds.Contains(e.Id))
                    .ToListAsync();
                
                // Filter events that match any search term
                var matchingEvents = keywordMatches
                    .Where(e => searchTerms.Any(term => 
                        e.Title.ToLower().Contains(term) || 
                        e.Description.ToLower().Contains(term) ||
                        e.Category.ToLower().Contains(term)))
                    .OrderBy(e => e.StartDate)
                    .Take(3 - recommended.Count);
                
                recommended.AddRange(matchingEvents);
            }
            
            // Strategy 3: Fallback to general upcoming events
            if (recommended.Count < 3)
            {
                var existingIds = recommended.Select(e => e.Id).ToHashSet();
                var fallback = _upcomingEventsQueue.UnorderedItems
                    .Select(x => x.Element)
                    .Where(e => !existingIds.Contains(e.Id))
                    .OrderBy(e => e.StartDate)
                    .Take(3 - recommended.Count)
                    .ToList();
                recommended.AddRange(fallback);
            }
            
            return recommended.Distinct().Take(3).ToList();
        }
    }
}
