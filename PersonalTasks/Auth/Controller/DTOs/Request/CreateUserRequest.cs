using System.ComponentModel.DataAnnotations;

namespace PersonalTasks.Auth.Controller.DTOs.Request
{
    public record CreateUserRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 50 characters.")]
        public required string Password { get; set; } = null!;
    }
}
