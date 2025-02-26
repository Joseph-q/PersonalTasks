using PersonalTasks.Auth.Controller.DTOs.Request;
using PersonalTasks.Models;

namespace PersonalTasks.Auth.Services
{
    public interface IAuthService
    {
        Task RegisterUser(CreateUserRequest user);
        Task<string> GenerateJWT(User user);
        Task<User?> GetUserWithPasswordByUsername(string username);
    }
}
