using System.Collections.Generic;

namespace Municipal_Servcies_Portal.Models
{
    public class Issue
    {
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string>? AttachmentPaths { get; set; } = new List<string>();
        public DateTime DateReported { get; set; } = DateTime.Now;
        
        // Notification preferences
        public string? NotificationEmail { get; set; }
        public string? NotificationPhone { get; set; }
    }
}
