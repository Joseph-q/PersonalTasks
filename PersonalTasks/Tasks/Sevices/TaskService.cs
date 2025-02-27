using Microsoft.EntityFrameworkCore;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;

namespace PersonalTasks.Tasks.Sevices
{
    public class TaskService(ContextDb context) : ITaskService
    {

        private readonly ContextDb _context = context;

        public Task CreateTask(TaskItem task)
        {
            _context.TaskItems.Add(task);
            return _context.SaveChangesAsync();
        }

        public Task DeleteTask(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
            return _context.SaveChangesAsync();
        }

        public Task<TaskItem?> GetTask(int id)
        {
            return _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<TaskItem?> GetUserTask(int taskId, int userId)
        {
            return _context.TaskItems.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId.Equals(userId));
        }

        public Task<List<TaskItem>> GetUserTasks(GetTasksQueryParams queryParams)
        {
            var chain = _context.TaskItems.AsQueryable();

            if (queryParams.Completed.HasValue)
            {
                chain = chain.Where(t => t.Completed == queryParams.Completed);
            }

            if (queryParams.CreatedAt != null)
            {
                chain = chain.Where(t => t.CreatedAt == queryParams.CreatedAt);
            }

            if (queryParams.UpdatedAt != null)
            {
                chain = chain.Where(t => t.UpdatedAt == queryParams.UpdatedAt);
            }


            return chain.Where(t => t.UserId == queryParams.UserId).ToListAsync();


        }

        public Task UpdateTask(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            return _context.SaveChangesAsync();
        }
    }
}
