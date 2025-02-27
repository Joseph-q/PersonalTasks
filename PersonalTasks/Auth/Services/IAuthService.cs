using PersonalTasks.Models;

namespace PersonalTasks.Auth.Services
{
    public interface IAuthService
    {
        Task RegisterUser(User user);
        string GenerateJWT(User user);
        Task<User?> GetUserWithPasswordByUsername(string username);
    }
}
