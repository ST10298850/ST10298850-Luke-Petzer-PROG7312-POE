using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Data;
using Municipal_Servcies_Portal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session support for search history tracking
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Session expires after 2 hours of inactivity
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add HttpContextAccessor for session access
builder.Services.AddHttpContextAccessor();

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MunicipalDB")));

// Register IssueService as Scoped (requires DbContext)
builder.Services.AddScoped<IssueService>();

// Register LocalEventsService (Phase 2)
builder.Services.AddScoped<ILocalEventsService, LocalEventsService>();

// Register SearchHistoryService for recommendation tracking
builder.Services.AddScoped<SearchHistoryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware (must be before UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();