# Municipal Services Portal

A comprehensive web application for reporting municipal issues, discovering local events and announcements, and staying connected with your community. Built with ASP.NET Core MVC and Entity Framework Core.

## Link to Repository

https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git

## Link to YouTube Video

https://youtu.be/6EByHpbRAZs

## Features

### 🔧 Report Issues
- Submit municipal issues (potholes, streetlights, graffiti, etc.) with location, category, and description
- Attach multiple files (images, PDFs) to reports
- **Database persistence** - all issues stored in SQL Server
- Optional progress notifications via email or SMS
- Unique reference numbers for tracking (format: `#MSP-2025-000001`)
- Status tracking (Pending, InProgress, Resolved, Closed)
- Confirmation page with detailed report summary

### 📅 Local Events and Announcements
- **Advanced search and filtering** by event name, category, and date
- **Intelligent recommendation system** based on user search patterns
- **Auto-submit filters** with debounced search (no button clicks needed)
- **Session-based search history** tracking for personalized recommendations
- View upcoming community events with detailed information
- Municipal announcements sidebar
- **8 event categories**: Government Meetings, Community Events, Public Safety, Parks & Recreation, Cultural Events, Educational Events, Health & Wellness, Holiday Events

### 🎯 Recommendation Engine
- **4-strategy recommendation algorithm**:
  1. Category-based recommendations (most searched category)
  2. Keyword-based recommendations (matching search terms)
  3. Date-based recommendations (from frequently searched dates)
  4. Fallback to upcoming events
- Analyzes user search patterns and preferences
- Tracks last 10 searches per user session
- Displays top 3 personalized recommendations

### 🎨 Modern UI/UX
- Responsive design with custom CSS styling
- Progress bars and loading animations
- Drag-and-drop file uploads
- Color-coded event categories
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
  - Custom CSS (see `/wwwroot/css/`)
  - JavaScript (ES6+)
  - Auto-submit forms with debouncing

- **Data Structures (Advanced):**
  - `SortedDictionary<DateTime, List<Event>>` - Events by date
  - `Dictionary<string, List<Event>>` - Events by category
  - `HashSet<string>` - Unique categories
  - `HashSet<DateTime>` - Unique event dates
  - `PriorityQueue<Event, DateTime>` - Upcoming events prioritization
  - `Stack<Event>` - Recently viewed events
  - `Queue<SearchHistory>` - User search tracking (session-based)

- **Design Patterns:**
  - MVC (Model-View-Controller)
  - Repository Pattern (Service Layer)
  - Dependency Injection
  - Async/Await for all database operations

## Project Structure

```
Municipal-Servcies-Portal/
├── Controllers/
│   ├── HomeController.cs
│   ├── IssueController.cs          # Database-backed issue reporting
│   └── LocalEventsController.cs    # Events, filtering, recommendations
├── Data/
│   ├── AppDbContext.cs             # EF Core DbContext
│   └── DbSeeder.cs                 # Seeds 25+ events, 10 announcements
├── Migrations/
│   ├── 20251014160425_InitialCreate.cs
│   └── 20251015092707_AddIssuesTable.cs
├── Models/
│   ├── Issue.cs                    # Database entity with validation
│   ├── Event.cs                    # Event entity
│   ├── Announcement.cs             # Announcement entity
│   └── ErrorViewModel.cs
├── Services/
│   ├── IssueService.cs             # Database CRUD for issues
│   ├── LocalEventsService.cs       # Advanced data structures & search
│   ├── ILocalEventsService.cs      # Service interface
│   └── SearchHistoryService.cs     # Session-based search tracking
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
│   │   └── Index.cshtml            # Search, filter, recommendations
│   └── Shared/
│       ├── _Layout.cshtml
│       └── Error.cshtml
├── wwwroot/
│   ├── css/
│   │   ├── site.css
│   │   ├── home.css
│   │   ├── issues.css
│   │   ├── localevents.css
│   │   └── confirmation.css
│   ├── images/icons/
│   ├── js/
│   │   ├── site.js
│   │   └── localevents.js
│   └── uploads/                    # User-uploaded files
├── appsettings.json                # Connection strings
├── Program.cs                      # DI, services, middleware
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

### Events Table
- `Id` (int, PK, Identity)
- `Title` (nvarchar(200)) - Event name
- `Description` (nvarchar(2000)) - Event details
- `Category` (nvarchar(100)) - Event category
- `StartDate` (datetime2) - Event start
- `EndDate` (datetime2) - Event end
- `ImagePath` (nvarchar(500)) - Optional image
- `IsActive` (bit) - Active flag

### Announcements Table
- `Id` (int, PK, Identity)
- `Title` (nvarchar(200))
- `Content` (nvarchar(2000))
- `DatePosted` (datetime2)
- `IsActive` (bit)

**Indexes:** Created on DateReported, Status, Category, StartDate for optimized queries

## How It Works

### Backend Architecture

#### Issue Reporting (Database-Backed)
1. **Model:** `Issue` entity with validation attributes and JSON serialization for attachments
2. **Service:** `IssueService` uses Entity Framework Core for async CRUD operations
3. **Controller:** `IssueController` handles form submission, file uploads, and database persistence
4. **Storage:** SQL Server database with automatic ID generation and reference numbers

#### Local Events & Recommendations
1. **Data Structures:** `LocalEventsService` loads events into advanced data structures:
   - SortedDictionary for chronological ordering
   - Dictionary for category-based lookups
   - HashSet for unique values
   - PriorityQueue for upcoming events
   - Stack for recently viewed events

2. **Search & Filter:** Async database queries with multiple filter criteria:
   - Text search (partial match, case-insensitive)
   - Category filter (exact match)
   - Date range filter (events from date onwards)

3. **Recommendation Engine:**
   - Tracks user searches in session storage (Queue structure)
   - Analyzes last 10 searches for patterns
   - Frequency analysis on categories and dates
   - Keyword matching in event titles/descriptions
   - 4-tier fallback strategy

4. **Session Management:**
   - `SearchHistoryService` stores search history per user
   - 2-hour session timeout
   - JSON serialization for complex data types

### Frontend Features

#### Issue Reporting Form
- **Progress Bar:** Dynamic calculation based on filled fields
- **File Upload:** Drag-and-drop with multiple file support
- **Validation:** Client-side and server-side validation
- **Confirmation:** Displays unique reference number from database

#### Local Events Page
- **Auto-Submit Filters:** Forms submit automatically on change
- **Debounced Search:** 500ms delay to reduce server requests
- **Recommendations Section:** Top 3 events based on user behavior
- **Color-Coded Categories:** Visual distinction between event types
- **Responsive Grid:** Events and announcements in two-column layout

## Setup & Running

### Prerequisites
- .NET 8.0 SDK
- **SQL Server** (choose one option below):
  - **Option 1 (Recommended for Windows):** SQL Server LocalDB (included with Visual Studio)
  - **Option 2:** SQL Server Express (free, standalone installation)
  - **Option 3:** Full SQL Server instance
  - **Option 4 (macOS/Linux):** SQL Server in Docker
- Visual Studio 2022 / Rider / VS Code

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git
   cd Municipal-Servcies-Portal
   ```

2. **Configure Database Connection:**

   Open `appsettings.json` and update the connection string based on your setup:

   #### **Option 1: SQL Server LocalDB (Windows - Recommended)**
   
   **Perfect for:** Visual Studio users on Windows
   
   **Setup:** No installation needed - included with Visual Studio
   
   Replace the `ConnectionStrings` section with:
   ```json
   "ConnectionStrings": {
     "MunicipalDB": "Server=(localdb)\\mssqllocaldb;Database=MunicipalServicesDB;Trusted_Connection=True;MultipleActiveResultSets=true"
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
     "MunicipalDB": "Server=.\\SQLEXPRESS;Database=MunicipalServicesDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```

   ---

   #### **Option 3: Full SQL Server (Windows)**
   
   **Perfect for:** Users with SQL Server already installed
   
   Replace the `ConnectionStrings` section with:
   ```json
   "ConnectionStrings": {
     "MunicipalDB": "Server=localhost;Database=MunicipalServicesDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```

   ---

   #### **Option 4: SQL Server in Docker (macOS/Linux)**
   
   **Perfect for:** macOS and Linux users
   
   **Setup Docker Container:**
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" \
     -p 1433:1433 --name sql_server \
     -d mcr.microsoft.com/mssql/server:2022-latest
   ```
   
   Replace the `ConnectionStrings` section with:
   ```json
   "ConnectionStrings": {
     "MunicipalDB": "Server=localhost,1433;Database=MunicipalServicesDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
   }
   ```
   
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
   Build started...
   Build succeeded.
   Applying migration '20251014160425_InitialCreate'.
   Applying migration '20251015092707_AddIssuesTable'.
   Done.
   ```
   
   **What this does:**
   - Creates the `MunicipalServicesDB` database
   - Creates three tables: `Issues`, `Events`, `Announcements`
   - Seeds 25 events and 10 announcements automatically
   - Sets up all indexes and constraints

5. **Build and Run:**
   ```bash
   dotnet run
   ```
   
   Or if using Visual Studio, press **F5** or click **Run**.

6. **Open in Browser:**
   
   Navigate to the URL shown in the terminal (typically):
   - **HTTPS:** `https://localhost:5298`
   - **HTTP:** `http://localhost:5000`

### Verify Database Setup

After running migrations, verify your database was created:

**For LocalDB/SQL Server (Windows):**
```bash
# Using SQL Server Management Studio (SSMS)
# Connect to: (localdb)\mssqllocaldb
# Database: MunicipalServicesDB

# Or using command line:
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT COUNT(*) FROM MunicipalServicesDB.dbo.Events"
```

**For Docker (macOS/Linux):**
```bash
# Connect to container
docker exec -it sql_server /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P 'YourStrong!Passw0rd' \
  -Q "SELECT COUNT(*) FROM MunicipalServicesDB.dbo.Events"
```

**Expected:** Should return `25` (number of seeded events)

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
1. Click **"Report Issues"** on the home page
2. Fill in required fields:
   - Location (street address or landmark)
   - Category (select from dropdown)
   - Description (detailed explanation)
3. Optionally:
   - Upload files (drag-and-drop or click to browse)
   - Add notification email/phone for updates
4. Submit → Issue saved to database with unique ID
5. View confirmation page with reference number (e.g., `#MSP-2025-000001`)

### Browsing Local Events
1. Click **"Local Events and Announcements"** on home page
2. Use filters to search:
   - **Search box:** Type event keywords (e.g., "Summer", "Yoga")
   - **Category dropdown:** Filter by event type
   - **Date picker:** Show events from specific date onwards
3. Filters auto-submit (no button needed)
4. View **"Recommended for You"** section at top:
   - Based on your search history
   - Updates as you search more
5. Clear filters to see all upcoming events

### How Recommendations Work
- Search for events by text or category
- System tracks your searches in session
- Recommendations prioritize:
  1. Your most searched category
  2. Events matching your search keywords
  3. Events near your searched dates
  4. General upcoming events (fallback)
- Recommendations persist during your session (2 hours)

## Assignment Requirements Met

### Phase 1: Report Issues ✅
- [x] MVC Pattern with clean separation of concerns
- [x] Database persistence with Entity Framework Core
- [x] File uploads (multiple files supported)
- [x] Progress bar (dynamic, animated)
- [x] Frontend/Backend integration
- [x] Navigation and responsive UI
- [x] Status tracking infrastructure

### Phase 2: Local Events & Announcements ✅
- [x] Display upcoming events aesthetically
- [x] Search functionality (name, category, date)
- [x] Advanced data structures:
  - [x] Stacks (recently viewed events)
  - [x] Queues (search history tracking)
  - [x] Priority Queues (upcoming events)
  - [x] Hash Tables/Dictionaries (event organization)
  - [x] Sorted Dictionaries (chronological ordering)
  - [x] Sets (unique categories/dates)

### Phase 3: Recommendation Feature ✅
- [x] Analyze user search patterns
- [x] Multi-strategy recommendation algorithm
- [x] Session-based history tracking
- [x] User-friendly presentation
- [x] Frequency analysis for preferences

## Code Quality Features

- **Async/Await:** All database operations are async
- **Dependency Injection:** Services properly registered
- **Data Validation:** Model validation attributes
- **Error Handling:** Try-catch blocks and null checks
- **Soft Deletes:** IsActive flags instead of hard deletes
- **Indexes:** Database indexes for performance
- **Comments:** Comprehensive XML documentation
- **Separation of Concerns:** Service layer abstraction

## Future Enhancements (Planned)

- [ ] Issue status tracking dashboard
- [ ] Admin panel for managing issues
- [ ] Email notifications for issue updates
- [ ] User authentication and authorization
- [ ] Issue search and filtering
- [ ] Real-time status updates
- [ ] Reporting and analytics
- [ ] Mobile app integration

## Database Seeding

The application automatically seeds the database with:
- **25 diverse events** across 8 categories spanning 30 days
- **10 municipal announcements** with recent dates
- Events include: Town halls, festivals, workshops, safety training, cultural events, health screenings, etc.

## Troubleshooting

### Database Connection Issues
```bash
# Verify SQL Server is running
# Update connection string in appsettings.json
# Re-run migrations
dotnet ef database drop
dotnet ef database update
```

### Migration Errors
```bash
# Remove existing migrations (if needed)
dotnet ef migrations remove

# Create fresh migration
dotnet ef migrations add InitialCreate

# Apply to database
dotnet ef database update
```

### Session Not Persisting
- Ensure `app.UseSession()` is before `app.UseAuthorization()` in Program.cs
- Check browser cookies are enabled
- Session timeout is set to 2 hours

## Technologies & NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Session" Version="2.2.0" />
```

## Performance Optimizations

- Database indexes on frequently queried columns
- Async operations to prevent thread blocking
- Debounced search to reduce server load
- Session caching for search history
- Efficient data structures (O(log n) lookups)
- Lazy loading of event data structures

## License

This project is for educational purposes as part of PROG7312 coursework.

---

**Developed by:** Luke Petzer | ST10298850  
**Institution:** The Independent Institute of Education  
**Module:** PROG7312 - Programming 2B  
**Assignment:** Municipal Services Portal (POE)  
**Date:** October 2025

---

## Contact

For questions or issues, please contact:
- **Student:** Luke Petzer
- **Student Number:** ST10298850
- **GitHub:** https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE

---

*"Empowering communities through technology and civic engagement."*
