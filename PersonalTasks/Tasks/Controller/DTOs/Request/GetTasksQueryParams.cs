using System.ComponentModel.DataAnnotations;

namespace PersonalTasks.Tasks.Controller.DTOs.Request
{
    public record GetTasksQueryParams
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
        public int Page { get; init; } = 1;

        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100.")]
        public int Limit { get; init; } = 20;

        public bool? Completed { get; init; }

        public string OrderBy { get; init; } = "Id";

        public string Order { get; init; } = "asc";

        public DateOnly? CreatedAt { get; init; }

    }
}
