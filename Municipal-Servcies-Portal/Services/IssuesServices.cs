// IssueService.cs
// Service layer for managing reported issues using Entity Framework Core for database persistence.
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Data;

namespace Municipal_Servcies_Portal.Services
{
    /// <summary>
    /// Provides methods to add, retrieve, and manage issues with database persistence.
    /// Replaces in-memory Queue storage with Entity Framework Core database operations.
    /// </summary>
    public class IssueService
    {
        private readonly AppDbContext _context;

        public IssueService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new issue to the database.
        /// </summary>
        /// <param name="issue">The issue to add.</param>
        public async Task AddIssueAsync(Issue issue)
        {
            issue.DateReported = DateTime.Now;
            issue.Status = "Pending";
            issue.IsActive = true;
            
            await _context.Issues.AddAsync(issue);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all active issues from the database ordered by date reported (newest first).
        /// </summary>
        /// <returns>A list of all active issues.</returns>
        public async Task<IEnumerable<Issue>> GetAllIssuesAsync()
        {
            return await _context.Issues
                .Where(i => i.IsActive)
                .OrderByDescending(i => i.DateReported)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific issue by ID.
        /// </summary>
        /// <param name="id">The issue ID.</param>
        /// <returns>The issue if found, null otherwise.</returns>
        public async Task<Issue?> GetIssueByIdAsync(int id)
        {
            return await _context.Issues
                .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);
        }

        /// <summary>
        /// Retrieves issues filtered by category.
        /// </summary>
        /// <param name="category">The category to filter by.</param>
        /// <returns>A list of issues in the specified category.</returns>
        public async Task<IEnumerable<Issue>> GetIssuesByCategoryAsync(string category)
        {
            return await _context.Issues
                .Where(i => i.IsActive && i.Category == category)
                .OrderByDescending(i => i.DateReported)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves issues filtered by status.
        /// </summary>
        /// <param name="status">The status to filter by (e.g., "Pending", "InProgress", "Resolved").</param>
        /// <returns>A list of issues with the specified status.</returns>
        public async Task<IEnumerable<Issue>> GetIssuesByStatusAsync(string status)
        {
            return await _context.Issues
                .Where(i => i.IsActive && i.Status == status)
                .OrderByDescending(i => i.DateReported)
                .ToListAsync();
        }

        /// <summary>
        /// Updates an existing issue's status.
        /// </summary>
        /// <param name="id">The issue ID.</param>
        /// <param name="newStatus">The new status.</param>
        public async Task UpdateIssueStatusAsync(int id, string newStatus)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue != null && issue.IsActive)
            {
                issue.Status = newStatus;
                issue.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Soft deletes an issue by marking it as inactive.
        /// </summary>
        /// <param name="id">The issue ID.</param>
        public async Task DeleteIssueAsync(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue != null)
            {
                issue.IsActive = false;
                issue.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets the count of issues by status.
        /// </summary>
        /// <returns>Dictionary with status as key and count as value.</returns>
        public async Task<Dictionary<string, int>> GetIssueCountsByStatusAsync()
        {
            return await _context.Issues
                .Where(i => i.IsActive)
                .GroupBy(i => i.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        /// <summary>
        /// Gets recent issues (last 30 days).
        /// </summary>
        /// <returns>List of recent issues.</returns>
        public async Task<IEnumerable<Issue>> GetRecentIssuesAsync()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            return await _context.Issues
                .Where(i => i.IsActive && i.DateReported >= thirtyDaysAgo)
                .OrderByDescending(i => i.DateReported)
                .ToListAsync();
        }
    }
}