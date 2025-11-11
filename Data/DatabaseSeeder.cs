using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models;
using TaskStatusEnum = TaskApi.Models.TaskStatus;

namespace TaskApi.Data;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(TaskDbContext context)
    {
        // Check if database is empty
        if (await context.Tasks.AnyAsync())
        {
            return; // Database already has data
        }
        
        // Create sample tasks
        var sampleTasks = new List<TaskItem>
        {
            // NOT_STARTED tasks
            new TaskItem
            {
                Title = "Set up development environment",
                Description = "Install necessary tools and configure the development environment for the project",
                Status = TaskStatusEnum.NOT_STARTED,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new TaskItem
            {
                Title = "Create project documentation",
                Description = "Write comprehensive documentation for the project including API specs and user guides",
                Status = TaskStatusEnum.NOT_STARTED,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                UpdatedAt = DateTime.UtcNow.AddDays(-4)
            },
            
            // IN_PROGRESS tasks
            new TaskItem
            {
                Title = "Implement user authentication",
                Description = "Develop login, registration, and password reset functionality",
                Status = TaskStatusEnum.IN_PROGRESS,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Design database schema",
                Description = "Create and optimize database tables for the application",
                Status = TaskStatusEnum.IN_PROGRESS,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddHours(-6)
            },
            
            // COMPLETED tasks
            new TaskItem
            {
                Title = "Set up CI/CD pipeline",
                Description = "Configure automated testing and deployment processes",
                Status = TaskStatusEnum.COMPLETED,
                CreatedAt = DateTime.UtcNow.AddDays(-6),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Create task management API",
                Description = "Develop REST API endpoints for task CRUD operations",
                Status = TaskStatusEnum.COMPLETED,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddHours(-2)
            }
        };
        
        await context.Tasks.AddRangeAsync(sampleTasks);
        await context.SaveChangesAsync();
        
        Console.WriteLine($"✅ Database seeded with {sampleTasks.Count} sample tasks");
    }
}
