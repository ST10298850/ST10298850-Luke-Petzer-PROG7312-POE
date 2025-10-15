using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Municipal_Servcies_Portal.Models
{
    /// <summary>
    /// Represents a citizen-reported issue in the municipal services system
    /// Stored in database for persistent tracking and management
    /// </summary>
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        // Store attachment paths as JSON string in database
        [MaxLength(4000)]
        public string? AttachmentPathsJson { get; set; }
        
        // Helper property to work with attachments as a list (not mapped to DB)
        [NotMapped]
        public List<string> AttachmentPaths
        {
            get
            {
                if (string.IsNullOrEmpty(AttachmentPathsJson))
                    return new List<string>();
                
                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<string>>(AttachmentPathsJson) ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
            set
            {
                AttachmentPathsJson = value != null && value.Any() 
                    ? System.Text.Json.JsonSerializer.Serialize(value) 
                    : null;
            }
        }
        
        [Required]
        public DateTime DateReported { get; set; } = DateTime.Now;
        
        // Issue status tracking
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Resolved, Closed
        
        // Notification preferences
        [MaxLength(200)]
        [EmailAddress]
        public string? NotificationEmail { get; set; }
        
        [MaxLength(20)]
        [Phone]
        public string? NotificationPhone { get; set; }
        
        // Tracking fields
        public DateTime? LastUpdated { get; set; }
        
        [MaxLength(100)]
        public string? AssignedTo { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
