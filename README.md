# Municipal Services Portal

A web application for reporting municipal issues, tracking service requests, and staying connected with your community. Built with ASP.NET Core MVC.

##Link to Repository

https://github.com/ST10298850/ST10298850-Luke-Petzer-PROG7312-POE.git

##Link to YouTube Video

https://youtu.be/6EByHpbRAZs

## Features

- **Report Issues:**
  - Submit new municipal issues (e.g., potholes, streetlights, graffiti) with location, category, description, and attachments.
  - Attach multiple files (images, PDFs) to your report.
  - Optional progress notifications via email or SMS.
- **Track Requests:**
  - View confirmation and summary of submitted issues.
  - (Planned) Track the status of your requests.
- **Local Events:**
  - (Planned) Discover upcoming community events.
- **Responsive UI:**
  - Modern, accessible design with progress bars and loading animations.

## Technologies Used

- ASP.NET Core MVC (.NET 8)
- C#
- Razor Views
- Bootstrap (for layout)
- Custom CSS (see `/wwwroot/css/`)
- JavaScript (for progress bar, drag-and-drop, and UI enhancements)

## Project Structure

```
Municipal-Servcies-Portal/
├── Controllers/
│   ├── HomeController.cs
│   └── IssueController.cs
├── Models/
│   └── Issue.cs
├── Services/
│   └── IssuesServices.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── ComingSoon.cshtml
│   ├── Issue/
│   │   ├── Create.cshtml
│   │   └── Confirmation.cshtml
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/
│   ├── css/
│   ├── images/
│   ├── js/
│   └── uploads/
├── Program.cs
└── ...
```

## How It Works

### Backend
- **Model:** `Issue` holds all report data, including optional notification fields.
- **Service Layer:** `IssueService` manages a `Queue<Issue>` for first-come, first-served processing (no lists/arrays used).
- **Controller:** `IssueController` handles GET/POST for creating issues, file uploads, and confirmation display.
- **Dependency Injection:** Service registered in `Program.cs` as a singleton.

### Frontend
- **Form:** Located in `Views/Issue/Create.cshtml`. Fields: location, category, description, attachments, email/SMS opt-in.
- **Progress Bar:** JavaScript-driven, updates as user fills out required and optional fields.
- **Drag & Drop:** Attachments support drag-and-drop and multiple file selection.
- **Navigation:** Logo and back buttons redirect to home page.
- **Confirmation:** `Views/Issue/Confirmation.cshtml` displays submitted data and download links for attachments.

## Setup & Running

1. **Clone the repository:**
   ```bash
   git clone <repo-url>
   cd Municipal-Servcies-Portal
   ```
2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```
3. **Build and run:**
   ```bash
   dotnet run --framework net8.0
   ```
4. **Open in browser:**
   Visit [http://localhost:5000](http://localhost:5000) (or the port shown in your terminal).

## Usage

- Click **Get Started** on the home page to report an issue.
- Fill in all required fields. Attach files if needed.
- Opt in for progress notifications via email or SMS (optional).
- Submit the form. You’ll be redirected to a confirmation page.
- Use the navigation links or logo to return to the home page.

## Assignment Requirements Met

- [x] **MVC Pattern**: Clean separation of concerns.
- [x] **No Lists/Arrays**: Uses `Queue<Issue>` for storage.
- [x] **File Uploads**: Multiple files supported.
- [x] **Progress Bar**: Dynamic, animated, and reflects optional fields.
- [x] **Frontend/Backend Integration**: Fully functional.
- [x] **Navigation**: All buttons and logo redirect as required.
- [x] **Accessibility & Responsiveness**: Modern, user-friendly UI.

## Customization

- **Add new categories:** Edit the `<select>` in `Create.cshtml`.
- **Change notification logic:** Update the model and controller as needed.
- **Style updates:** Edit CSS in `/wwwroot/css/`.

## License

This project is for educational purposes. All icons and images are for demonstration only.

---

**Developed by:** Luke Petzer | ST10298850

**Assignment:** Municipal Services Portal



