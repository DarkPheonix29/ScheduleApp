using BLL.Interfaces;
using DAL;
using DAL.Services;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using BLL.Firebase;
using BLL.Manager;
using Google.Api;

var builder = WebApplication.CreateBuilder(args);

// --- Firebase Initialization ---
FirebaseApp.Create(new AppOptions
{
	Credential = GoogleCredential.FromFile("C:\\Users\\keala\\source\\repos\\ScheduleApp\\scheduleapp-819ca-firebase-adminsdk-hj5ct-ed6b7912e0.json")
});

// --- Configure Services ---

// Register Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlite(connectionString);
});

// Dependency Injection for Repositories and Managers
builder.Services.AddScoped<IFirebaseKeyManager, FirebaseKeyRepos>();
builder.Services.AddScoped<IEventRepos, EventRepos>();
builder.Services.AddScoped<IFirebaseTokenManager, FirebaseTokenManager>();


// Register custom Firebase-related services
builder.Services.AddScoped<IFirebaseUserRepos, FirebaseUserRepos>(); // Renamed service, ensure its functionality matches your use case
builder.Services.AddScoped<IUserManager, UserManager>();

// Add controllers and views
builder.Services.AddControllersWithViews();

// Add Authentication and Authorization Middleware
builder.Services.AddAuthentication("Firebase")
	.AddCookie("Firebase"); // Use cookie-based authentication for user sessions

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("RequireAuthenticatedUser", policy =>
	{
		policy.RequireAuthenticatedUser();
	});

// Add Swagger/OpenAPI for development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Build the App ---
var app = builder.Build();

// --- Apply Migrations ---
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate(); // Automatically apply pending migrations at startup

// --- Configure Middleware ---

// Serve static files and default files (e.g., index.html for SPA)
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Enable Authentication Middleware
app.UseAuthorization();  // Enable Authorization Middleware

// Add Role-based Middleware


// Map API Controllers and Fallback for SPA
app.MapControllers();
app.MapFallbackToFile("/index.html");

// --- Run the App ---
app.Run();
