using System.ComponentModel.DataAnnotations;

namespace PersonalTasks.Tasks.Controller.DTOs.Request
{
    public record UpdateTaskRequest
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
        public string? Title { get; init; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; init; }

        public bool? IsCompleted { get; init; }
    }
}
