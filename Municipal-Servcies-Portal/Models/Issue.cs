namespace Municipal_Servcies_Portal.Models
{
    public class Issue
    {
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AttachmentPath { get; set; }
        public DateTime DateReported { get; set; } = DateTime.Now;
        
        // Notification preferences
        public string? NotificationEmail { get; set; }
        public string? NotificationPhone { get; set; }
    }
}
