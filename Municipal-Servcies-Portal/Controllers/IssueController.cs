// IssueController.cs
// Handles issue creation, file uploads, and confirmation display.
using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Services;
using System.Text.Json;

namespace Municipal_Servcies_Portal.Controllers;

public class IssueController : Controller
{
    // Service for managing issues.
    private readonly IssueService _service;
    // Provides access to web root for file uploads.
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Constructor for dependency injection of IssueService and environment.
    /// </summary>
    public IssueController(IssueService service, IWebHostEnvironment env)
    {
        _service = service;
        _env = env;
    }

    /// <summary>
    /// Displays the issue creation form.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles form submission, file uploads, and redirects to confirmation.
    /// </summary>
    /// <param name="location">Location of the issue.</param>
    /// <param name="category">Category of the issue.</param>
    /// <param name="description">Description of the issue.</param>
    /// <param name="attachments">Uploaded files.</param>
    /// <param name="notificationEmail">Optional notification email.</param>
    /// <param name="notificationPhone">Optional notification phone.</param>
    [HttpPost]
    public IActionResult Create(string location, string category, string description, IFormFile[]? attachments, string? notificationEmail, string? notificationPhone)
    {
        var fileNames = new List<string>();
        if (attachments != null)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);
            foreach (var file in attachments)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(stream);
                    fileNames.Add(fileName);
                }
            }
        }
        var issue = new Issue
        {
            Location = location,
            Category = category,
            Description = description,
            AttachmentPaths = fileNames,
            NotificationEmail = string.IsNullOrWhiteSpace(notificationEmail) ? null : notificationEmail,
            NotificationPhone = string.IsNullOrWhiteSpace(notificationPhone) ? null : notificationPhone
        };
        _service.AddIssue(issue);
        // Store issue data in TempData for confirmation page.
        TempData["IssueLocation"] = issue.Location;
        TempData["IssueCategory"] = issue.Category;
        TempData["IssueDescription"] = issue.Description;
        TempData["IssueAttachments"] = JsonSerializer.Serialize(issue.AttachmentPaths);
        TempData["IssueDateReported"] = issue.DateReported.ToString("g");
        TempData["IssueNotificationEmail"] = issue.NotificationEmail;
        TempData["IssueNotificationPhone"] = issue.NotificationPhone;
        return RedirectToAction("Confirmation");
    }

    /// <summary>
    /// Displays the confirmation page with submitted issue details.
    /// </summary>
    public IActionResult Confirmation()
    {
        var attachmentsJson = TempData["IssueAttachments"]?.ToString();
        var attachments = string.IsNullOrEmpty(attachmentsJson) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(attachmentsJson);
        var issue = new Issue
        {
            Location = TempData["IssueLocation"]?.ToString() ?? "",
            Category = TempData["IssueCategory"]?.ToString() ?? "",
            Description = TempData["IssueDescription"]?.ToString() ?? "",
            AttachmentPaths = attachments,
            DateReported = DateTime.TryParse(TempData["IssueDateReported"]?.ToString(), out var date) ? date : DateTime.Now,
            NotificationEmail = TempData["IssueNotificationEmail"]?.ToString(),
            NotificationPhone = TempData["IssueNotificationPhone"]?.ToString()
        };

        return View(issue);
    }
}