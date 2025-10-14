using Municipal_Servcies_Portal.Data;
using Municipal_Servcies_Portal.Models;
using Microsoft.EntityFrameworkCore;

namespace Municipal_Servcies_Portal.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Check if data already exists
            if (await context.Events.AnyAsync() || await context.Announcements.AnyAsync())
            {
                return; // Database already seeded
            }

            // Seed Events
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Community Town Hall Meeting",
                    Description = "Join us for our monthly town hall to discuss community issues and upcoming projects.",
                    Category = "Government Meetings",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(5).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Summer Festival 2025",
                    Description = "Annual summer festival featuring live music, food vendors, and family activities.",
                    Category = "Community Events",
                    StartDate = DateTime.Now.AddDays(15),
                    EndDate = DateTime.Now.AddDays(17),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Public Safety Workshop",
                    Description = "Learn about emergency preparedness and safety measures for your family.",
                    Category = "Public Safety",
                    StartDate = DateTime.Now.AddDays(10),
                    EndDate = DateTime.Now.AddDays(10).AddHours(3),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Parks & Recreation Cleanup Day",
                    Description = "Volunteer to help clean and maintain our local parks. All supplies provided.",
                    Category = "Parks & Recreation",
                    StartDate = DateTime.Now.AddDays(7),
                    EndDate = DateTime.Now.AddDays(7).AddHours(4),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Cultural Heritage Festival",
                    Description = "Celebrate our diverse community with cultural performances, art exhibits, and cuisine.",
                    Category = "Cultural Events",
                    StartDate = DateTime.Now.AddDays(20),
                    EndDate = DateTime.Now.AddDays(21),
                    ImagePath = "",
                    IsActive = true
                }
            };

            // Seed Announcements
            var announcements = new List<Announcement>
            {
                new Announcement
                {
                    Title = "New Recycling Program Launched",
                    Content = "The city has launched a new recycling program. Blue bins will be distributed next week.",
                    DatePosted = DateTime.Now.AddDays(-2),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Road Construction on Main Street",
                    Content = "Main Street will be closed for repairs from Oct 20-25. Please use alternate routes.",
                    DatePosted = DateTime.Now.AddDays(-1),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "City Budget Meeting Results",
                    Content = "The annual budget has been approved. View details on our website.",
                    DatePosted = DateTime.Now.AddDays(-5),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "New Community Center Opening",
                    Content = "Our state-of-the-art community center will open its doors on November 1st.",
                    DatePosted = DateTime.Now.AddDays(-3),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Winter Weather Preparation",
                    Content = "As winter approaches, please ensure your property is prepared for snow and ice.",
                    DatePosted = DateTime.Now,
                    IsActive = true
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.Announcements.AddRangeAsync(announcements);
            await context.SaveChangesAsync();
        }
    }
}

