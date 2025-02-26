using System.ComponentModel.DataAnnotations;

namespace PersonalTasks.Auth.Controller.DTOs.Request
{
    public record LoginRequest
    {
        [Required]
        public required string Username { get; set; } = null!;

        [Required]
        public required string Password { get; set; } = null!;
    }
}
