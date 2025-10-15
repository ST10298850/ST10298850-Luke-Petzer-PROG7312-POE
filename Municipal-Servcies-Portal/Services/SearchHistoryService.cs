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
        private const int MaxSearchHistory = 7;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchHistoryService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the user's search history from session storage
        /// </summary>
        public List<SearchHistoryItem> GetSearchHistory()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return new List<SearchHistoryItem>();

            var json = session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json))
                return new List<SearchHistoryItem>();

            try
            {
                var list = JsonSerializer.Deserialize<List<SearchHistoryItem>>(json);
                return list ?? new List<SearchHistoryItem>();
            }
            catch
            {
                return new List<SearchHistoryItem>();
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
            history.Add(new SearchHistoryItem
            {
                SearchText = searchText,
                Category = category,
                Date = date
            });

            // Maintain max size - remove oldest entries
            if (history.Count > MaxSearchHistory)
            {
                history.RemoveRange(0, history.Count - MaxSearchHistory);
            }

            // Save back to session
            SaveSearchHistory(history);
        }

        /// <summary>
        /// Save search history to session storage
        /// </summary>
        private void SaveSearchHistory(List<SearchHistoryItem> history)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return;

            var json = JsonSerializer.Serialize(history);
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

        /// <summary>
        /// Helper class for JSON serialization
        /// </summary>
        public class SearchHistoryItem
        {
            public string? SearchText { get; set; }
            public string? Category { get; set; }
            public DateTime? Date { get; set; }
        }
    }
}
