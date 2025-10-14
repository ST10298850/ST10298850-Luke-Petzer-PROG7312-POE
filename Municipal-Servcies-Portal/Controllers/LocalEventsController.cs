using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Services;
using System.Text.Json;

namespace Municipal_Servcies_Portal.Controllers;

public class LocalEventsController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
}