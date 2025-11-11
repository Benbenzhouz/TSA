using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models;
using TaskApi.DTOs;
using System.ComponentModel.DataAnnotations;
using TaskStatusEnum = TaskApi.Models.TaskStatus;

namespace TaskApi.Endpoints;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/tasks").WithTags("Tasks");

        // GET /tasks
        group.MapGet("/", async (TaskDbContext db, string? status) =>
        {
            var query = db.Tasks.AsQueryable();
            
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<TaskStatusEnum>(status, true, out var statusEnum))
                {
                    query = query.Where(t => t.Status == statusEnum);
                }
                else
                {
                    return Results.BadRequest(new { message = "Invalid status. Valid values: NOT_STARTED, IN_PROGRESS, COMPLETED" });
                }
            }
            
            var tasks = await query
                .OrderBy(t => t.Id)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status.ToString(),
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();
            
            return Results.Ok(tasks);
        })
        .WithName("GetTasks")
        .WithSummary("Get all tasks")
        .WithDescription("Get all tasks with optional status filter");

        // POST /tasks
        group.MapPost("/", async (TaskDbContext db, CreateTaskDto createDto) =>
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(createDto.Title))
            {
                return Results.BadRequest(new { message = "Title is required and cannot be empty" });
            }
            
            if (createDto.Title.Length > 120)
            {
                return Results.BadRequest(new { message = "Title cannot exceed 120 characters" });
            }
            
            // Parse status
            TaskStatusEnum status = TaskStatusEnum.NOT_STARTED;
            if (!string.IsNullOrEmpty(createDto.Status))
            {
                if (!Enum.TryParse<TaskStatusEnum>(createDto.Status, true, out status))
                {
                    return Results.BadRequest(new { message = "Invalid status. Valid values: NOT_STARTED, IN_PROGRESS, COMPLETED" });
                }
            }
            
            var task = new TaskItem
            {
                Title = createDto.Title.Trim(),
                Description = createDto.Description?.Trim(),
                Status = status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            
            var responseDto = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
            
            return Results.Created($"/tasks/{task.Id}", responseDto);
        })
        .WithName("CreateTask")
        .WithSummary("Create a new task")
        .WithDescription("Create a new task with title (required), description (optional), and status (optional, defaults to NOT_STARTED)");

        // PUT /tasks/{id}
        group.MapPut("/{id:int}", async (TaskDbContext db, int id, UpdateTaskDto updateDto) =>
        {
            var task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return Results.NotFound(new { message = $"Task with id {id} not found" });
            }
            
            bool isUpdated = false;
            
            // Update title if provided
            if (!string.IsNullOrWhiteSpace(updateDto.Title))
            {
                if (updateDto.Title.Length > 120)
                {
                    return Results.BadRequest(new { message = "Title cannot exceed 120 characters" });
                }
                task.Title = updateDto.Title.Trim();
                isUpdated = true;
            }
            
            // Update description if provided (including empty string to clear it)
            if (updateDto.Description != null)
            {
                task.Description = string.IsNullOrWhiteSpace(updateDto.Description) ? null : updateDto.Description.Trim();
                isUpdated = true;
            }
            
            // Update status if provided
            if (!string.IsNullOrEmpty(updateDto.Status))
            {
                if (Enum.TryParse<TaskStatusEnum>(updateDto.Status, true, out var status))
                {
                    task.Status = status;
                    isUpdated = true;
                }
                else
                {
                    return Results.BadRequest(new { message = "Invalid status. Valid values: NOT_STARTED, IN_PROGRESS, COMPLETED" });
                }
            }
            
            if (isUpdated)
            {
                task.UpdatedAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
            
            var responseDto = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
            
            return Results.Ok(responseDto);
        })
        .WithName("UpdateTask")
        .WithSummary("Update a task")
        .WithDescription("Update a task's title, description, and/or status");

        // DELETE /tasks/{id}
        group.MapDelete("/{id:int}", async (TaskDbContext db, int id) =>
        {
            var task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return Results.NotFound(new { message = $"Task with id {id} not found" });
            }
            
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            
            return Results.NoContent();
        })
        .WithName("DeleteTask")
        .WithSummary("Delete a task")
        .WithDescription("Delete a task by its ID");
    }
}
