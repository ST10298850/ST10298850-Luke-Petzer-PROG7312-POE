// IssuesServices.cs
// Service layer for managing reported issues using a Queue for FIFO order.
using System.Collections.Generic;
using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Services
{
    /// <summary>
    /// Provides methods to add and retrieve issues in a first-in, first-out manner.
    /// </summary>
    public class IssueService
    {
        // Queue to store issues in the order they are received (FIFO).
        private readonly Queue<Issue> _issues = new();

        /// <summary>
        /// Adds a new issue to the queue.
        /// </summary>
        /// <param name="issue">The issue to add.</param>
        public void AddIssue(Issue issue)
        {
            _issues.Enqueue(issue);
        }

        /// <summary>
        /// Retrieves all issues currently in the queue.
        /// </summary>
        /// <returns>An IEnumerable of all issues.</returns>
        public IEnumerable<Issue> GetAllIssues()
        {
            return _issues;
        }
    }
}