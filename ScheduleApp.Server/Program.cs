using BLL.Interfaces;
using BLL.Managers;
using DAL;
using DAL.Services;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IStudentRepos, StudentRepos>();
builder.Services.AddScoped<IEventRepos, EventRepos>();
builder.Services.AddScoped<IEventManager, EventManager>();
builder.Services.AddScoped<FirebaseRoles>();  // Register renamed service
builder.Services.AddScoped<FirebaseKey>();

// OR for SQLite (if you prefer)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlite(connectionString);
});

// Initialize Firebase Admin SDK
FirebaseApp.Create(new AppOptions()
{
	Credential = GoogleCredential.FromFile("C:\\Users\\keala\\source\\repos\\ScheduleApp\\scheduleapp-819ca-firebase-adminsdk-hj5ct-ed6b7912e0.json")
});

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();

