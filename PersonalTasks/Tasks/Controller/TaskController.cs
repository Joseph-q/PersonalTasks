using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;
using PersonalTasks.Tasks.Controller.DTOs.Response;
using PersonalTasks.Tasks.Sevices;
using System.Security.Claims;

namespace PersonalTasks.Tasks.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    public class TaskController(ITaskService taskService, IMapper mapper) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            int parsedUserId = int.Parse(userId);


            TaskItem? taskItem = await _taskService.GetUserTask(id, parsedUserId);
            if (taskItem == null)
            {
                return NotFound();
            }
            TaskResponse res = _mapper.Map<TaskResponse>(taskItem);

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] GetTasksQueryParams queryParams)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (userId == null)
            {
                return Unauthorized();
            }

            queryParams.UserId = int.Parse(userId);

            if (queryParams.UserId == 0)
            {
                return BadRequest("UserId is required.");
            }

            IEnumerable<TaskItem> taskItems = await _taskService.GetUserTasks(queryParams);
            IEnumerable<TaskResponse> res = _mapper.Map<IEnumerable<TaskResponse>>(taskItems);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest taskRequest)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (userId == null)
            {
                return Unauthorized();
            }

            TaskItem taskItem = _mapper.Map<TaskItem>(taskRequest);
            taskItem.UserId = int.Parse(userId);


            await _taskService.CreateTask(taskItem);
            return CreatedAtAction(nameof(GetTask), new { id = taskItem.Id }, taskItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest taskRequest)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (userId == null)
            {
                return Unauthorized();
            }
            int parsedUserId = int.Parse(userId);

            TaskItem? taskItem = await _taskService.GetUserTask(id, parsedUserId);


            if (taskItem == null)
            {
                return NotFound();
            }
            _mapper.Map(taskRequest, taskItem);



            await _taskService.UpdateTask(taskItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (userId == null)
            {
                return Unauthorized();
            }
            int parsedUserId = int.Parse(userId);

            TaskItem? taskItem = await _taskService.GetUserTask(id, parsedUserId);
            if (taskItem == null)
            {
                return NotFound();
            }
            await _taskService.DeleteTask(taskItem);
            return NoContent();

        }
    }
}