using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.ViewModels
{
    public class LocalEventsViewModel
    {
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
        public IEnumerable<Announcement> Announcements { get; set; } = new List<Announcement>();
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public IEnumerable<Event> RecentlyViewedEvents { get; set; } = new List<Event>();
        public IEnumerable<DateTime> UniqueEventDates { get; set; } = new List<DateTime>();
        public IEnumerable<Event> RecommendedEvents { get; set; } = new List<Event>();
    }
}
