using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// OR for SQLite (if you prefer)
builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlite("Data Source=local.db")); // Or your preferred database configuration
builder.Services.AddScoped<IStudentService>();

//services.AddDbContext<DbContext>(options =>
//options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
