using System.Text.Json;

namespace Municipal_Servcies_Portal.Services
{
    /// <summary>
    /// Service to manage user search history across HTTP requests using session storage
    /// This ensures recommendations can track user patterns over time
    /// </summary>
    public class SearchHistoryService
    {
        private const string SessionKey = "UserSearchHistory";
        private const int MaxSearchHistory = 10;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchHistoryService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the user's search history from session storage
        /// </summary>
        public Queue<(string? SearchText, string? Category, DateTime? Date)> GetSearchHistory()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return new Queue<(string? SearchText, string? Category, DateTime? Date)>();

            var json = session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json))
                return new Queue<(string? SearchText, string? Category, DateTime? Date)>();

            try
            {
                var list = JsonSerializer.Deserialize<List<SearchHistoryItem>>(json);
                if (list == null) return new Queue<(string? SearchText, string? Category, DateTime? Date)>();

                var queue = new Queue<(string? SearchText, string? Category, DateTime? Date)>();
                foreach (var item in list)
                {
                    queue.Enqueue((item.SearchText, item.Category, item.Date));
                }
                return queue;
            }
            catch
            {
                return new Queue<(string? SearchText, string? Category, DateTime? Date)>();
            }
        }

        /// <summary>
        /// Add a new search to the user's history
        /// </summary>
        public void AddSearch(string? searchText, string? category, DateTime? date)
        {
            // Track if at least one parameter has a value
            if (string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(category) && !date.HasValue)
                return; // Don't track empty searches

            var history = GetSearchHistory();
            
            // Add new search
            history.Enqueue((searchText, category, date));

            // Maintain max size
            while (history.Count > MaxSearchHistory)
            {
                history.Dequeue();
            }

            // Save back to session
            SaveSearchHistory(history);
        }

        /// <summary>
        /// Save search history to session storage
        /// </summary>
        private void SaveSearchHistory(Queue<(string? SearchText, string? Category, DateTime? Date)> history)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return;

            var list = history.Select(h => new SearchHistoryItem 
            { 
                SearchText = h.SearchText,
                Category = h.Category, 
                Date = h.Date 
            }).ToList();

            var json = JsonSerializer.Serialize(list);
            session.SetString(SessionKey, json);
        }

        /// <summary>
        /// Clear the user's search history
        /// </summary>
        public void ClearHistory()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Remove(SessionKey);
        }

        // Helper class for JSON serialization (tuples don't serialize well)
        private class SearchHistoryItem
        {
            public string? SearchText { get; set; }
            public string? Category { get; set; }
            public DateTime? Date { get; set; }
        }
    }
}
