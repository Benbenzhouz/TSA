using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(120, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public TaskStatus Status { get; set; } = TaskStatus.NOT_STARTED;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}