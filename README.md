# Municipal Services Portal

A comprehensive web application for reporting municipal issues, discovering local events and announcements, and staying connected with your community. Built with ASP.NET Core MVC and Entity Framework Core.

## Link to Repository

https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git

## Link to YouTube Video
[
https://youtu.be/6EByHpbRAZs](https://youtu.be/Y9FKudbk_m0)

## Features

### Report Issues
- Submit municipal issues (potholes, streetlights, graffiti, etc.) with location, category, and description
- Attach multiple files (images, PDFs) to reports
- **Database persistence** - all issues stored in SQL Server
- Optional progress notifications via email or SMS
- Unique reference numbers for tracking (format: `#MSP-2025-000001`)
- Status tracking (Pending, InProgress, Resolved, Closed)
- Confirmation page with detailed report summary

### Local Events and Announcements
- **Advanced search and filtering** by event name, category, and date
- **Intelligent recommendation system** based on user search patterns
- **Auto-submit filters** with debounced search (700ms delay - no button clicks needed)
- **Session-based search history** tracking for personalized recommendations
- View upcoming community events with detailed information and realistic times
- Municipal announcements sidebar with recent updates
- **8 event categories**: Government Meetings, Community Events, Public Safety, Parks & Recreation, Cultural Events, Educational Events, Health & Wellness, Holiday Events
- **Bootstrap Icons** integration for clean, professional UI elements

### Recommendation Engine
- **Multi-strategy recommendation algorithm**:
  1. Category-based recommendations (most frequently searched category)
  2. Keyword-based recommendations (matching search terms in title/description)
  3. Fallback to upcoming events
- Analyzes user search patterns and preferences
- Tracks last 10 searches per user session
- Displays top 3 personalized recommendations
- Session storage with 2-hour timeout

### Modern UI/UX
- Responsive design with custom CSS styling
- **Bootstrap Icons** for consistent iconography
- Progress bars and loading animations
- Drag-and-drop file uploads
- Color-coded event categories (blue, green, orange badges)
- Clean, accessible navigation

## Technologies Used

- **Backend:**
  - ASP.NET Core MVC (.NET 8)
  - C# 12
  - Entity Framework Core 8.0.10
  - SQL Server (Database)
  - Async/Await pattern throughout

- **Frontend:**
  - Razor Views
  - Bootstrap 5
  - Bootstrap Icons 1.11.1 (CDN)
  - Custom CSS (see `/wwwroot/css/`)
  - JavaScript (ES6+)
  - Auto-submit forms with debouncing

- **Data Structures (Advanced):**
  - `SortedDictionary<DateTime, List<Event>>` - Events by date (O(log n) lookups)
  - `Dictionary<string, List<Event>>` - Events by category (O(1) lookups)
  - `HashSet<string>` - Unique categories
  - `HashSet<DateTime>` - Unique event dates
  - `PriorityQueue<Event, DateTime>` - Upcoming events prioritization
  - `Stack<Event>` - Recently viewed events (LIFO)
  - `List<SearchHistoryItem>` - User search tracking (session-based, simplified)

- **Design Patterns:**
  - MVC (Model-View-Controller)
  - Repository Pattern (Service Layer)
  - Dependency Injection
  - Async/Await for all database operations
  - Session management for user state

## Project Structure

```
Municipal-Servcies-Portal/
├── Controllers/
│   ├── HomeController.cs
│   ├── IssueController.cs          # Database-backed issue reporting
│   └── LocalEventsController.cs    # Events, filtering, recommendations
├── Data/
│   ├── AppDbContext.cs             # EF Core DbContext
│   └── DbSeeder.cs                 # Seeds 27 events with times, 10 announcements
├── Migrations/
│   ├── 20251014160425_InitialCreate.cs
│   └── 20251015092707_AddIssuesTable.cs
├── Models/
│   ├── Issue.cs                    # Database entity with validation
│   ├── Event.cs                    # Event entity (formerly Events.cs)
│   ├── Announcement.cs             # Announcement entity
│   └── ErrorViewModel.cs
├── Services/
│   ├── IssuesServices.cs           # Database CRUD for issues
│   ├── LocalEventsService.cs       # Advanced data structures & search
│   ├── ILocalEventsService.cs      # Service interface
│   └── SearchHistoryService.cs     # Simplified session-based tracking
├── ViewModels/
│   └── LocalEventsViewModel.cs     # Composite view model
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── ComingSoon.cshtml
│   ├── Issue/
│   │   ├── Create.cshtml
│   │   └── Confirmation.cshtml     # Shows DB reference number
│   ├── LocalEvents/
│   │   └── Index.cshtml            # Search, filter, recommendations w/ Bootstrap Icons
│   └── Shared/
│       ├── _Layout.cshtml          # Includes Bootstrap Icons CDN
│       ├── _ValidationScriptsPartial.cshtml
│       └── Error.cshtml
├── wwwroot/
│   ├── css/
│   │   ├── site.css
│   │   ├── home.css
│   │   ├── issues.css
│   │   ├── localevents.css
│   │   ├── confirmation.css
│   │   └── comingsoon.css
│   ├── images/icons/
│   ├── js/
│   │   ├── site.js
│   │   └── localevents.js
│   └── uploads/                    # User-uploaded files
├── appsettings.json                # Connection strings
├── appsettings.Development.json
├── Program.cs                      # DI, services, middleware, session config
└── Municipal-Servcies-Portal.csproj
```

## Database Schema

### Issues Table
- `Id` (int, PK, Identity) - Unique identifier
- `Location` (nvarchar(200)) - Issue location
- `Category` (nvarchar(100)) - Issue type
- `Description` (nvarchar(2000)) - Detailed description
- `AttachmentPathsJson` (nvarchar(4000)) - JSON array of file paths
- `DateReported` (datetime2) - Submission timestamp
- `Status` (nvarchar(50)) - Pending/InProgress/Resolved/Closed
- `NotificationEmail` (nvarchar(200)) - Optional email
- `NotificationPhone` (nvarchar(20)) - Optional phone
- `LastUpdated` (datetime2) - Last modification timestamp
- `AssignedTo` (nvarchar(100)) - Assigned staff member
- `IsActive` (bit) - Soft delete flag
- **Indexes:** Category, DateReported, Status

### Events Table
- `Id` (int, PK, Identity)
- `Title` (nvarchar(200)) - Event name
- `Description` (nvarchar(2000)) - Event details
- `Category` (nvarchar(100)) - Event category
- `StartDate` (datetime2) - Event start (includes time)
- `EndDate` (datetime2) - Event end (includes time)
- `ImagePath` (nvarchar(500)) - Optional image
- `IsActive` (bit) - Active flag

### Announcements Table
- `Id` (int, PK, Identity)
- `Title` (nvarchar(max))
- `Content` (nvarchar(max))
- `DatePosted` (datetime2)
- `IsActive` (bit)

## How It Works

### Backend Architecture

#### Issue Reporting (Database-Backed)
1. **Model:** `Issue` entity with validation attributes and JSON serialization for attachments
2. **Service:** `IssuesServices` uses Entity Framework Core for async CRUD operations
3. **Controller:** `IssueController` handles form submission, file uploads, and database persistence
4. **Storage:** SQL Server database with automatic ID generation and reference numbers

#### Local Events & Recommendations
1. **Data Structures:** `LocalEventsService` loads events into advanced data structures:
   - SortedDictionary for chronological ordering
   - Dictionary for category-based lookups (O(1))
   - HashSet for unique values
   - PriorityQueue for upcoming events
   - Stack for recently viewed events

2. **Search & Filter:** Async database queries with multiple filter criteria:
   - Text search (partial match, case-insensitive)
   - Category filter (exact match)
   - Date range filter (events from date onwards)
   - Auto-submit with 500ms debounce

3. **Recommendation Engine:**
   - Tracks user searches in session storage (List structure)
   - Analyzes last 10 searches for patterns
   - Frequency analysis on categories
   - Keyword matching in event titles/descriptions/categories
   - Multi-tier fallback strategy

4. **Session Management:**
   - `SearchHistoryService` stores search history per user (simplified List-based)
   - 2-hour session timeout
   - JSON serialization for session storage
   - Maximum 10 searches tracked

### Frontend Features

#### Issue Reporting Form
- **Progress Bar:** Dynamic calculation based on filled fields
- **File Upload:** Drag-and-drop with multiple file support
- **Validation:** Client-side and server-side validation
- **Confirmation:** Displays unique reference number from database

#### Local Events Page
- **Auto-Submit Filters:** Forms submit automatically on change (debounced 500ms)
- **Bootstrap Icons:** Professional icons for search, calendar, announcements
  - `bi-search` - Search functionality
  - `bi-calendar-event` - Recommendation dates
  - `bi-calendar3` - Event dates
  - `bi-megaphone` - Announcements
- **Recommendations Section:** Top 3 events based on user behavior
- **Color-Coded Categories:** 
  - Blue: Government Meetings, Community Events
  - Green: Parks & Recreation, Utilities
  - Orange: Cultural Events
- **Responsive Grid:** Events and announcements in two-column layout

## Setup & Running

### Prerequisites
- .NET 8.0 SDK
- **SQL Server** (choose one option below):
  - **Option 1 (Recommended for Windows):** SQL Server LocalDB (included with Visual Studio)
  - **Option 2:** SQL Server Express (free, standalone installation)
  - **Option 3:** Full SQL Server instance
  - **Option 4 (macOS/Linux):** SQL Server in Docker
- Visual Studio 2022 / JetBrains Rider / VS Code

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git
   cd ST10298850-Luke-Petzer-PROG7312-POE/Municipal-Servcies-Portal
   ```

2. **Configure Database Connection:**

   Open `appsettings.json` and update the connection string based on your setup:

   #### **Option 1: SQL Server LocalDB (Windows - Recommended)**
   
   **Perfect for:** Visual Studio users on Windows
   
   **Setup:** No installation needed - included with Visual Studio
   
   Replace the `ConnectionStrings` section with:
   ```json
   "ConnectionStrings": {
     "MunicipalDB": "Server=(localdb)\\mssqllocaldb;Database=MunicipalServices;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
   
   **Verify LocalDB is installed:**
   ```bash
   sqllocaldb info
   ```
   
   If not installed, you can install it with:
   - Visual Studio Installer → Modify → Individual Components → SQL Server Express LocalDB

   ---

   #### **Option 2: SQL Server Express (Windows)**
   
   **Perfect for:** Windows users who want a standalone SQL Server
   
   **Setup:** Download and install [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   
   Replace the `ConnectionStrings` section with:
   ```json
   "ConnectionStrings": {
     "MunicipalDB": "Server=.\\SQLEXPRESS;Database=MunicipalServices;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```

   ---

   #### **Option 3: Full SQL Server (Windows)**
   
   **Perfect for:** Users with SQL Server already installed
   
   Replace the `ConnectionStrings` section with:
   ```json
   "ConnectionStrings": {
     "MunicipalDB": "Server=localhost;Database=MunicipalServices;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```

   ---
   
   **Note:** Replace `YourStrong!Passw0rd` with your actual password set in the Docker command.

3. **Restore Dependencies:**
   ```bash
   dotnet restore
   ```

4. **Apply Database Migrations:**
   
   This will create the database and all tables automatically:
   ```bash
   dotnet ef database update
   ```
   
   **Expected Output:**
   ```
   Build succeeded.
   Applying migration '20251014160425_InitialCreate'.
   Applying migration '20251015092707_AddIssuesTable'.
   Done.
   ```
   
   **What this does:**
   - Creates the `MunicipalServices` database
   - Creates three tables: `Issues`, `Events`, `Announcements`
   - Seeds 27 events with realistic times and 10 announcements automatically
   - Sets up all indexes and constraints

5. **Build and Run:**
   ```bash
   dotnet run
   ```
   
   Or if using Visual Studio/Rider, press **F5** or click **Run**.

6. **Open in Browser:**
   
   Navigate to the URL shown in the terminal (typically):
   - **HTTPS:** `https://localhost:5001` or `https://localhost:5298`
   - **HTTP:** `http://localhost:5000`

### Verify Database Setup

After running migrations, verify your database was created:

**For LocalDB/SQL Server (Windows):**
```bash
# Using SQL Server Management Studio (SSMS)
# Connect to: (localdb)\mssqllocaldb
# Database: MunicipalServices

# Or using command line:
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT COUNT(*) FROM MunicipalServices.dbo.Events"
```

**Expected:** Should return `27` (number of seeded events)

### Common Database Setup Issues

#### Issue: "Cannot open database"
**Solution:** Run migrations again:
```bash
dotnet ef database update
```

#### Issue: "Login failed for user"
**Solution (Docker):** Check your password matches in both appsettings.json and Docker command

#### Issue: "LocalDB not found"
**Solution:** Install LocalDB via Visual Studio Installer or use SQL Server Express instead

#### Issue: "A network-related error occurred"
**Solution:** 
- For LocalDB: Start LocalDB instance: `sqllocaldb start mssqllocaldb`
- For Docker: Ensure container is running: `docker ps`
- For SQL Server: Check SQL Server service is running

#### Issue: "Database already exists" (when switching setups)
**Solution:** Drop and recreate:
```bash
dotnet ef database drop --force
dotnet ef database update
```

## Usage Guide

### Reporting an Issue
1. Click **"Report Issues"** on the home page or navigation
2. Fill in required fields:
   - Location (street address or landmark)
   - Category (select from dropdown)
   - Description (detailed explanation)
3. Optionally:
   - Upload files (drag-and-drop or click to browse, supports multiple files)
   - Add notification email/phone for updates
4. Watch the progress bar fill as you complete fields
5. Submit → Issue saved to database with unique ID
6. View confirmation page with reference number (e.g., `#MSP-2025-000001`)

### Browsing Local Events
1. Click **"Local Events"** on the navigation menu
2. Use filters to search (all filters auto-submit):
   - **Search box:** Type event keywords (e.g., "Farmers", "Yoga", "Town Hall")
   - **Category dropdown:** Filter by event type (Community Events, Parks & Recreation, etc.)
   - **Date picker:** Show events from specific date onwards
3. View events with realistic times (e.g., "Saturday, October 19, 2025 • 6:00 PM")
4. See **"Recommended for You"** section at top:
   - Based on your search history
   - Updates as you search more
   - Shows top 3 personalized events
5. Click "Clear Filters" to reset and see all upcoming events
6. Browse municipal announcements in the right sidebar

### How Recommendations Work
- Search for events by text or category
- System tracks your searches in session storage
- Recommendations prioritize:
  1. **Category-based**: Events in your most searched category
  2. **Keyword-based**: Events matching your search terms
  3. **Upcoming fallback**: General upcoming events if not enough matches
- Recommendations persist during your session (2 hours)
- Privacy-focused: Search history stored only in your session, not database

## Code Quality Features

- **Async/Await:** All database operations are async
- **Dependency Injection:** Services properly registered in Program.cs
- **Data Validation:** Model validation attributes
- **Error Handling:** Try-catch blocks and null checks
- **Soft Deletes:** IsActive flags instead of hard deletes
- **Indexes:** Database indexes for performance optimization
- **Comments:** Comprehensive XML documentation
- **Separation of Concerns:** Service layer abstraction
- **Session Management:** Proper session configuration
- **Clean UI:** Bootstrap Icons instead of custom SVG

## Database Seeding

The application automatically seeds the database with:
- **27 diverse events** across 8 categories spanning 30 days
- **Realistic event times**: Morning workshops (8-9 AM), evening meetings (6-9 PM), all-day festivals, etc.
- **10 municipal announcements** with recent dates
- Events include: Town halls, festivals, farmers markets, safety workshops, cultural events, health screenings, and more

## Troubleshooting

### Database Connection Issues
```bash
# Verify SQL Server is running
# Update connection string in appsettings.json
# Re-run migrations
dotnet ef database drop --force
dotnet ef database update
```

### Migration Errors
```bash
# Check existing migrations
dotnet ef migrations list

# Apply to database
dotnet ef database update
```

### Session Not Persisting
- Ensure `app.UseSession()` is before `app.UseAuthorization()` in Program.cs
- Check browser cookies are enabled
- Session timeout is set to 2 hours (120 minutes)

### Bootstrap Icons Not Showing
- Check internet connection (icons loaded from CDN)
- Verify `_Layout.cshtml` includes Bootstrap Icons CDN link
- Clear browser cache and refresh

## Technologies & NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Session" Version="2.2.0" />
```

**External Libraries:**
- Bootstrap 5 (CSS Framework)
- Bootstrap Icons 1.11.1 (Icon Library - CDN)
- jQuery 3.x (JavaScript Library)

## References

Microsoft. (2024) ASP.NET Core documentation. Available at: https://learn.microsoft.com/en-us/aspnet/core/ (Accessed: 10 October 2025).

Microsoft. (2024) Entity Framework Core overview. Available at: https://learn.microsoft.com/en-us/ef/core/ (Accessed: 10 October 2025).

W3C. (2023) HTML Living Standard. Available at: https://html.spec.whatwg.org/ (Accessed: 10 October 2025).

Bootstrap. (2024) Bootstrap 5 Documentation. Available at: https://getbootstrap.com/docs/5.3/ (Accessed: 10 October 2025).

Jakobsen, L. (2023) ‘Responsive grid design for modern municipal portals’, International Journal of Web Design Systems, 18(2), pp. 74–89. Available at: https://doi.org/10.1016/ijwds.2023.02.007.



