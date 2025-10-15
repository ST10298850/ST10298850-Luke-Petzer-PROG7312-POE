using System.Collections.Generic;

namespace Municipal_Servcies_Portal.Models
{
    public class Event {
        public int Id {get;set;}
        public string Title {get;set;} = string.Empty;
        public string Description {get;set;} = string.Empty;
        public string Category {get;set;} = string.Empty;
        public DateTime StartDate {get;set;}
        public DateTime? EndDate {get;set;}
        public string ImagePath {get;set;} = string.Empty;
        public bool IsActive {get;set;} = true;
    }
    
    public class Announcement { 
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
