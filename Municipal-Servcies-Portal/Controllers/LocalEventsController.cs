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
    
    public async Task<IActionResult> Index()
    {
        var viewModel = new LocalEventsViewModel
        {
            Events = await _localEventsService.GetUpcomingEventsAsync(),
            Announcements = await _localEventsService.GetAllAnnouncementsAsync(),
            Categories = await _localEventsService.GetCategoriesAsync()
        };
        
        return View(viewModel);
    }
}