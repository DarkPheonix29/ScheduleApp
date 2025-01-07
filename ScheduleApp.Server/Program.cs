using BLL.Interfaces;
using DAL;
using DAL.repos;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using BLL.Firebase;
using BLL.Manager;
using Google.Api;
using DAL.repos;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// --- Firebase Initialization ---
var firebaseKeyPath = Environment.GetEnvironmentVariable("FIREBASE_KEY_PATH") ?? builder.Configuration["Firebase:ServiceAccountKeyPath"];
if (string.IsNullOrEmpty(firebaseKeyPath) || !File.Exists(firebaseKeyPath))
{
	throw new FileNotFoundException($"Firebase service account key file not found at '{firebaseKeyPath}'.");
}

FirebaseApp.Create(new AppOptions
{
	Credential = GoogleCredential.FromFile(firebaseKeyPath)
});


// Register Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlite(connectionString);
});
builder.Services.AddScoped<FirebaseAuth>(_ => FirebaseAuth.DefaultInstance);


// Dependency Injection for Repositories and Managers
builder.Services.AddScoped<IFirebaseKeyManager, FirebaseKeyRepos>();
builder.Services.AddScoped<IInstructorAvailabilityRepos, InstructorAvailabilityRepos>();
builder.Services.AddScoped<IStudentLessonRepos, StudentLessonRepos>();
builder.Services.AddScoped<IFirebaseTokenManager, FirebaseTokenManager>();
builder.Services.AddScoped<IProfileRepos, ProfileRepos>();
builder.Services.AddScoped<IEventManager, EventManager>();

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
