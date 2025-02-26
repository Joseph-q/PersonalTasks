using PersonalTasks.Auth.Controller.DTOs.Request;
using PersonalTasks.Models;

namespace PersonalTasks.Auth.Services
{
    public class AuthService : IAuthService
    {
        public Task<string> GenerateJWT(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserWithPasswordByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task RegisterUser(CreateUserRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
