using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

// Add CORS for frontend (包括静态文件端口)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", 
                          "http://localhost:5234", "http://127.0.0.1:5234")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Task Management API", 
        Version = "v1",
        Description = "A simple API for managing tasks"
    });
});

var app = builder.Build();

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    await context.Database.EnsureCreatedAsync();
    await DatabaseSeeder.SeedDatabaseAsync(context);
}

// Configure the HTTP request pipeline
// 添加静态文件支持
app.UseStaticFiles();

// CORS 必须在路由之前
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
        c.RoutePrefix = "api-docs"; // 改为 api-docs，让根路径给前端
    });
}

app.UseHttpsRedirection();

// Map task endpoints
app.MapTasksEndpoints();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    service = "Task Management API"
}))
.WithName("HealthCheck")
.WithSummary("Health check endpoint");

// 根路径重定向到前端页面
app.MapGet("/", () => Results.Redirect("/index.html"));

Console.WriteLine("🚀 Task Management API is starting...");
Console.WriteLine("📱 Frontend available at: http://localhost:5234");
Console.WriteLine("📖 Swagger UI available at: http://localhost:5234/api-docs");
Console.WriteLine("🔗 API base URL: http://localhost:5234/tasks");

app.Run();
