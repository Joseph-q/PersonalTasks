using System.ComponentModel.DataAnnotations;

namespace PersonalTasks.Tasks.Controller.DTOs.Request
{
    public record CreateTaskRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
        public required string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
