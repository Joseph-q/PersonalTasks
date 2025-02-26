using PersonalTasks.Models;

namespace PersonalTasks.Users.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByUsername(string username);

        Task<User?> GetUserById(int id);

        Task CreateUser(User user);

        Task UpdateUser(User user);

        Task DeleteUser(User user);
    }
}
