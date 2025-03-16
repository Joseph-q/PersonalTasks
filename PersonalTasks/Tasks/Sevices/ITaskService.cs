using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;
using PersonalTasks.Tasks.Controller.DTOs.Response;

namespace PersonalTasks.Tasks.Sevices
{
    public interface ITaskService
    {
        Task CreateTaskAsync(TaskItem taskItem);
        Task<TaskItem?> GetTaskAsync(int id, int? userId = null);
        Task<List<TaskItem>> GetListTasksAsync(GetTasksQueryParams queryParams, int? userId = null);

        Task<List<TaskResponse>> GetListTaskResponseAsync(GetTasksQueryParams queryParams, int? userId = null);
        Task<TaskResponse?> GetTaskResponseAsync(int id, int? userId = null);

        Task UpdateTaskAsync(TaskItem taskItem);
        Task DeleteTaskAsync(TaskItem taskItem);

    }
}
