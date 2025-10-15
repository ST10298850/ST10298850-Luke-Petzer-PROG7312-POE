// IssueController.cs
// Handles issue creation, file uploads, and confirmation display with database persistence.
using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Services;
using System.Text.Json;

namespace Municipal_Servcies_Portal.Controllers;

public class IssueController : Controller
{
    // Service for managing issues with database persistence.
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
    /// Handles form submission, file uploads, saves to database, and redirects to confirmation.
    /// </summary>
    /// <param name="location">Location of the issue.</param>
    /// <param name="category">Category of the issue.</param>
    /// <param name="description">Description of the issue.</param>
    /// <param name="attachments">Uploaded files.</param>
    /// <param name="notificationEmail">Optional notification email.</param>
    /// <param name="notificationPhone">Optional notification phone.</param>
    [HttpPost]
    public async Task<IActionResult> Create(string location, string category, string description, 
        IFormFile[]? attachments, string? notificationEmail, string? notificationPhone)
    {
        // Handle file uploads
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
                    await file.CopyToAsync(stream);
                    fileNames.Add(fileName);
                }
            }
        }
        
        // Create issue object
        var issue = new Issue
        {
            Location = location,
            Category = category,
            Description = description,
            AttachmentPaths = fileNames,
            NotificationEmail = string.IsNullOrWhiteSpace(notificationEmail) ? null : notificationEmail,
            NotificationPhone = string.IsNullOrWhiteSpace(notificationPhone) ? null : notificationPhone
        };
        
        // Save to database
        await _service.AddIssueAsync(issue);
        
        // Store issue ID in TempData for confirmation page
        TempData["IssueId"] = issue.Id;
        
        return RedirectToAction("Confirmation", new { id = issue.Id });
    }

    /// <summary>
    /// Displays the confirmation page with submitted issue details from database.
    /// </summary>
    /// <param name="id">The issue ID to display.</param>
    public async Task<IActionResult> Confirmation(int id)
    {
        var issue = await _service.GetIssueByIdAsync(id);
        
        if (issue == null)
        {
            return RedirectToAction("Create");
        }

        return View(issue);
    }
}