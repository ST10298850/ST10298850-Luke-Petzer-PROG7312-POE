using System.Collections.Generic;
using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Services
{
    public class IssueService
    {
        private readonly Queue<Issue> _issues = new();

        public void AddIssue(Issue issue)
        {
            _issues.Enqueue(issue);
        }

        public IEnumerable<Issue> GetAllIssues()
        {
            return _issues;
        }
    }
}