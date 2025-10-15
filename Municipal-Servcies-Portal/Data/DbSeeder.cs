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
                // Government Meetings
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
                    Title = "City Council Budget Session",
                    Description = "Public session to review and discuss the proposed annual budget for the upcoming fiscal year.",
                    Category = "Government Meetings",
                    StartDate = DateTime.Now.AddDays(12),
                    EndDate = DateTime.Now.AddDays(12).AddHours(3),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Zoning Board Public Hearing",
                    Description = "Public hearing for proposed zoning changes in the downtown district.",
                    Category = "Government Meetings",
                    StartDate = DateTime.Now.AddDays(18),
                    EndDate = DateTime.Now.AddDays(18).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Community Events
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
                    Title = "Farmers Market Opening Day",
                    Description = "Kick off the season with fresh local produce, artisan goods, and live entertainment.",
                    Category = "Community Events",
                    StartDate = DateTime.Now.AddDays(8),
                    EndDate = DateTime.Now.AddDays(8).AddHours(5),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Neighborhood Block Party",
                    Description = "Join your neighbors for food, games, and community fun on Oak Street.",
                    Category = "Community Events",
                    StartDate = DateTime.Now.AddDays(22),
                    EndDate = DateTime.Now.AddDays(22).AddHours(6),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Community Garage Sale",
                    Description = "City-wide garage sale event. Register your sale or come treasure hunting!",
                    Category = "Community Events",
                    StartDate = DateTime.Now.AddDays(30),
                    EndDate = DateTime.Now.AddDays(30).AddHours(8),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Public Safety
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
                    Title = "Fire Safety Training",
                    Description = "Free fire safety and prevention training for residents. Learn how to use fire extinguishers.",
                    Category = "Public Safety",
                    StartDate = DateTime.Now.AddDays(14),
                    EndDate = DateTime.Now.AddDays(14).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "CPR and First Aid Certification",
                    Description = "Get certified in CPR and basic first aid. Limited spots available, registration required.",
                    Category = "Public Safety",
                    StartDate = DateTime.Now.AddDays(25),
                    EndDate = DateTime.Now.AddDays(25).AddHours(4),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Parks & Recreation
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
                    Title = "Youth Soccer League Registration",
                    Description = "Sign up for the fall youth soccer league. Ages 5-14 welcome.",
                    Category = "Parks & Recreation",
                    StartDate = DateTime.Now.AddDays(11),
                    EndDate = DateTime.Now.AddDays(11).AddHours(3),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Hiking Trail Grand Opening",
                    Description = "Join us for the grand opening of the new 5-mile scenic hiking trail at Riverside Park.",
                    Category = "Parks & Recreation",
                    StartDate = DateTime.Now.AddDays(16),
                    EndDate = DateTime.Now.AddDays(16).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Community Garden Workshop",
                    Description = "Learn organic gardening techniques and get plot assignments for the community garden.",
                    Category = "Parks & Recreation",
                    StartDate = DateTime.Now.AddDays(28),
                    EndDate = DateTime.Now.AddDays(28).AddHours(3),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Cultural Events
                new Event
                {
                    Title = "Cultural Heritage Festival",
                    Description = "Celebrate our diverse community with cultural performances, art exhibits, and cuisine.",
                    Category = "Cultural Events",
                    StartDate = DateTime.Now.AddDays(20),
                    EndDate = DateTime.Now.AddDays(21),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Art in the Park Exhibition",
                    Description = "Local artists showcase their work in an outdoor gallery. Free admission.",
                    Category = "Cultural Events",
                    StartDate = DateTime.Now.AddDays(13),
                    EndDate = DateTime.Now.AddDays(13).AddHours(6),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Jazz in the Square",
                    Description = "Live jazz music performances in the town square. Bring your blankets and chairs!",
                    Category = "Cultural Events",
                    StartDate = DateTime.Now.AddDays(19),
                    EndDate = DateTime.Now.AddDays(19).AddHours(4),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Poetry Night at the Library",
                    Description = "Open mic poetry reading and performance. All skill levels welcome.",
                    Category = "Cultural Events",
                    StartDate = DateTime.Now.AddDays(27),
                    EndDate = DateTime.Now.AddDays(27).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Educational Events
                new Event
                {
                    Title = "Digital Literacy Workshop",
                    Description = "Learn basic computer skills and internet safety. Free for seniors.",
                    Category = "Educational Events",
                    StartDate = DateTime.Now.AddDays(9),
                    EndDate = DateTime.Now.AddDays(9).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Small Business Development Seminar",
                    Description = "Expert advice on starting and growing your small business in our community.",
                    Category = "Educational Events",
                    StartDate = DateTime.Now.AddDays(17),
                    EndDate = DateTime.Now.AddDays(17).AddHours(3),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Financial Planning for Families",
                    Description = "Free financial literacy workshop covering budgeting, saving, and investing basics.",
                    Category = "Educational Events",
                    StartDate = DateTime.Now.AddDays(24),
                    EndDate = DateTime.Now.AddDays(24).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Health & Wellness
                new Event
                {
                    Title = "Free Health Screening Day",
                    Description = "Blood pressure, cholesterol, and diabetes screenings. No appointment needed.",
                    Category = "Health & Wellness",
                    StartDate = DateTime.Now.AddDays(6),
                    EndDate = DateTime.Now.AddDays(6).AddHours(5),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Yoga in the Park",
                    Description = "Free outdoor yoga classes every Saturday morning. Bring your own mat.",
                    Category = "Health & Wellness",
                    StartDate = DateTime.Now.AddDays(4),
                    EndDate = DateTime.Now.AddDays(4).AddHours(1),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Mental Health Awareness Seminar",
                    Description = "Learn about mental health resources and support systems available in our community.",
                    Category = "Health & Wellness",
                    StartDate = DateTime.Now.AddDays(21),
                    EndDate = DateTime.Now.AddDays(21).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                
                // Holiday Events
                new Event
                {
                    Title = "Halloween Costume Parade",
                    Description = "Family-friendly costume parade through downtown. Prizes for best costumes!",
                    Category = "Holiday Events",
                    StartDate = DateTime.Now.AddDays(16),
                    EndDate = DateTime.Now.AddDays(16).AddHours(2),
                    ImagePath = "",
                    IsActive = true
                },
                new Event
                {
                    Title = "Veterans Day Ceremony",
                    Description = "Honor our veterans with a memorial ceremony at the town square.",
                    Category = "Holiday Events",
                    StartDate = DateTime.Now.AddDays(27),
                    EndDate = DateTime.Now.AddDays(27).AddHours(1),
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
                },
                new Announcement
                {
                    Title = "Water Main Maintenance Schedule",
                    Content = "Scheduled water main maintenance will affect the north district on Oct 22 from 9 AM to 3 PM.",
                    DatePosted = DateTime.Now.AddDays(-4),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Library Expansion Project Approved",
                    Content = "The city council has approved funding for the library expansion. Construction begins in spring.",
                    DatePosted = DateTime.Now.AddDays(-6),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Free Wi-Fi in Public Parks",
                    Content = "All city parks now have free public Wi-Fi available. Connect to 'CityParks' network.",
                    DatePosted = DateTime.Now.AddDays(-8),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Leaf Collection Program Starting",
                    Content = "Curbside leaf collection begins November 1st. Please rake leaves to curb by 7 AM on collection day.",
                    DatePosted = DateTime.Now.AddDays(-1),
                    IsActive = true
                },
                new Announcement
                {
                    Title = "New Bike Lane Installation Complete",
                    Content = "The new protected bike lanes on Elm Street are now open. Thank you for your patience during construction.",
                    DatePosted = DateTime.Now.AddDays(-7),
                    IsActive = true
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.Announcements.AddRangeAsync(announcements);
            await context.SaveChangesAsync();
        }
    }
}
