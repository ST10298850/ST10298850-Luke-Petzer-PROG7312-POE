using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Services;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Controllers;

public class LocalEventsController : Controller
{
    private readonly ILocalEventsService _localEventsService;

    public LocalEventsController(ILocalEventsService localEventsService)
    {
        _localEventsService = localEventsService;
    }
    
    /// <summary>
    /// Main Index action that handles both initial page load and search requests
    /// Accepts search parameters from query string (GET request)
    /// </summary>
    /// <param name="searchName">The event name/keyword to search for</param>
    /// <param name="category">The category to filter by</param>
    /// <param name="date">The date to filter events by</param>
    /// <returns>View with filtered or all events</returns>
    public async Task<IActionResult> Index(string? searchName, string? category, DateTime? date)
    {
        IEnumerable<Event> events;
        
        // Check if any search parameters were provided
        // If at least one parameter has a value, perform a search
        if (!string.IsNullOrEmpty(searchName) || !string.IsNullOrEmpty(category) || date.HasValue)
        {
            // For date: show events FROM the selected date onwards (no upper limit)
            events = await _localEventsService.SearchEventsAsync(searchName, category, date, null);
            
            // Optionally record the search for recommendations feature
            // This helps track user search patterns for personalized recommendations
            await _localEventsService.RecordSearchAsync(searchName ?? "", category, date, null);
        }
        else
        {
            // No search parameters provided - show all upcoming events
            events = await _localEventsService.GetUpcomingEventsAsync();
        }

        // Fetch additional data structures for the view
        var recentlyViewed = _localEventsService.GetRecentlyViewedEvents();
        var uniqueDates = await _localEventsService.GetUniqueEventDatesAsync();
        var recommended = await _localEventsService.GetRecommendedEventsAsync();

        // Build the ViewModel with the filtered/all events, announcements, and categories
        var viewModel = new LocalEventsViewModel
        {
            Events = events,
            Announcements = await _localEventsService.GetAllAnnouncementsAsync(),
            Categories = await _localEventsService.GetCategoriesAsync(),
            RecentlyViewedEvents = recentlyViewed,
            UniqueEventDates = uniqueDates,
            RecommendedEvents = recommended
        };
        
        return View(viewModel);
    }
}