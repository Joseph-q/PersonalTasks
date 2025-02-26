using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;

namespace PersonalTasks.Tasks.Sevices
{
    public interface ITaskService
    {
        Task CreateTask(TaskItem taskItem);
        Task<TaskItem?> GetTask(int id);
        Task<List<TaskItem>> GetUserTasks(GetTasksQueryParams queryParams);
        Task UpdateTask(TaskItem taskItem);
        Task DeleteTask(TaskItem taskItem);
    }
}
