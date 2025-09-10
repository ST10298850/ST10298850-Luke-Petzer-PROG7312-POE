using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Services;

namespace Municipal_Servcies_Portal.Controllers;

public class IssueController : Controller
{
    private readonly IssueService _service;
    private readonly IWebHostEnvironment _env;

    public IssueController(IssueService service, IWebHostEnvironment env)
    {
        _service = service;
        _env = env;
    }

    // GET: /Issue/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Issue/Create
    [HttpPost]
    public IActionResult Create(string location, string category, string description, IFormFile? attachment, string? notificationEmail, string? notificationPhone)
    {
        string? fileName = null;

        if (attachment != null && attachment.Length > 0)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            fileName = Path.GetFileName(attachment.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            attachment.CopyTo(stream);
        }

        var issue = new Issue
        {
            Location = location,
            Category = category,
            Description = description,
            AttachmentPath = fileName,
            NotificationEmail = string.IsNullOrWhiteSpace(notificationEmail) ? null : notificationEmail,
            NotificationPhone = string.IsNullOrWhiteSpace(notificationPhone) ? null : notificationPhone
        };

        _service.AddIssue(issue);

        // Use TempData to pass the issue to confirmation page
        TempData["IssueLocation"] = issue.Location;
        TempData["IssueCategory"] = issue.Category;
        TempData["IssueDescription"] = issue.Description;
        TempData["IssueAttachment"] = issue.AttachmentPath;
        TempData["IssueDateReported"] = issue.DateReported.ToString("g");
        TempData["IssueNotificationEmail"] = issue.NotificationEmail;
        TempData["IssueNotificationPhone"] = issue.NotificationPhone;

        return RedirectToAction("Confirmation");
    }

    public IActionResult Confirmation()
    {
        // Create issue object from TempData for the view
        var issue = new Issue
        {
            Location = TempData["IssueLocation"]?.ToString() ?? "",
            Category = TempData["IssueCategory"]?.ToString() ?? "",
            Description = TempData["IssueDescription"]?.ToString() ?? "",
            AttachmentPath = TempData["IssueAttachment"]?.ToString(),
            DateReported = DateTime.TryParse(TempData["IssueDateReported"]?.ToString(), out var date) ? date : DateTime.Now,
            NotificationEmail = TempData["IssueNotificationEmail"]?.ToString(),
            NotificationPhone = TempData["IssueNotificationPhone"]?.ToString()
        };

        return View(issue);
    }
}