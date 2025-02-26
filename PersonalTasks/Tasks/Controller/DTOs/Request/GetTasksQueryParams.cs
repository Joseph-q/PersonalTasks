using System.ComponentModel.DataAnnotations;

namespace PersonalTasks.Tasks.Controller.DTOs.Request
{
    public record GetTasksQueryParams
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100.")]
        public int Limit { get; set; } = 20;

        public bool? Completed { get; init; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateOnly? CreatedAt { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateOnly? UpdatedAt { get; set; }
        public int UserId { get; set; }
    }
}
