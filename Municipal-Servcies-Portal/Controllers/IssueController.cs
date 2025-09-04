using Microsoft.AspNetCore.Mvc;

namespace Municipal_Servcies_Portal.Controllers;

public class IssueController : Controller
{
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Confirmation()
    {
        return View();
    }
}