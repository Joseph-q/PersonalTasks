using Microsoft.EntityFrameworkCore;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;
using PersonalTasks.Tasks.Controller.DTOs.Response;

namespace PersonalTasks.Tasks.Sevices
{
    public class TaskService(ContextDb context) : ITaskService
    {

        private readonly ContextDb _context = context;

        public Task CreateTaskAsync(TaskItem taskItem)
        {
            _context.TaskItems.Add(taskItem);
            return _context.SaveChangesAsync();
        }

        public Task DeleteTaskAsync(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
            return _context.SaveChangesAsync();
        }

        public Task<List<TaskItem>> GetListTasksAsync(GetTasksQueryParams queryParams, int? userId = null)
        {
            int? page = queryParams.Page;
            int? limit = queryParams.Limit;

            string? order = queryParams.Order;
            string? orderBy = queryParams.OrderBy;

            DateOnly? createdAt = queryParams.CreatedAt;

            bool? completed = queryParams.Completed;


            var chain = _context.TaskItems.AsQueryable();

            if (userId != null)
            {
                chain = chain.Where(t => t.UserId.Equals(userId));
            }

            if (userId.HasValue)
            {
                chain = chain.Where(t => t.UserId == userId.Value);
            }

            if (createdAt.HasValue)
            {
                chain = chain.Where(t => t.CreatedAt == createdAt.Value);
            }

            if (completed.HasValue)
            {
                chain = chain.Where(t => t.Completed == completed.Value);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                bool descending = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
                chain = descending
                    ? chain.OrderByDescending(t => EF.Property<object>(t, orderBy))
                    : chain.OrderBy(t => EF.Property<object>(t, orderBy));
            }

            if (page.HasValue && limit.HasValue)
            {
                int skip = (page.Value - 1) * limit.Value;
                chain = chain.Skip(skip).Take(limit.Value);
            }


            return chain.ToListAsync();
        }

        public Task<TaskItem?> GetTaskAsync(int id, int? userId = null)
        {
            var chain = _context.TaskItems.AsQueryable();

            if (userId != null)
            {
                chain = chain.Where(t => t.UserId.Equals(userId));
            }
            return chain.Where(t => t.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public Task<TaskResponse?> GetTaskResponseAsync(int id, int? userId = null)
        {
            var chain = _context.TaskItems.AsQueryable();

            if (userId != null)
            {
                chain = chain.Where(t => t.UserId.Equals(userId));
            }
            return chain.Where(t => t.Id.Equals(id)).Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.Completed,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).FirstOrDefaultAsync();
        }

        public Task<List<TaskResponse>> GetListTaskResponseAsync(GetTasksQueryParams queryParams, int? userId = null)
        {
            int? page = queryParams.Page;
            int? limit = queryParams.Limit;

            string? order = queryParams.Order;
            string? orderBy = queryParams.OrderBy;

            DateOnly? createdAt = queryParams.CreatedAt;

            bool? completed = queryParams.Completed;
            var chain = _context.TaskItems.AsQueryable();


            if (userId != null)
            {
                chain = chain.Where(t => t.UserId.Equals(userId));
            }

            if (userId.HasValue)
            {
                chain = chain.Where(t => t.UserId == userId.Value);
            }

            if (createdAt.HasValue)
            {
                chain = chain.Where(t => t.CreatedAt == createdAt.Value);
            }

            if (completed.HasValue)
            {
                chain = chain.Where(t => t.Completed == completed.Value);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                bool descending = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
                chain = descending
                    ? chain.OrderByDescending(t => EF.Property<object>(t, orderBy))
                    : chain.OrderBy(t => EF.Property<object>(t, orderBy));
            }

            if (page.HasValue && limit.HasValue)
            {
                int skip = (page.Value - 1) * limit.Value;
                chain = chain.Skip(skip).Take(limit.Value);
            }

            if (userId != null)
            {
                chain = chain.Where(t => t.UserId.Equals(userId));
            }
            return chain.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.Completed,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToListAsync();
        }

        public Task UpdateTaskAsync(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            return _context.SaveChangesAsync();
        }
    }
}
