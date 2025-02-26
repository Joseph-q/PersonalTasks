using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;
using PersonalTasks.Tasks.Controller.DTOs.Response;
using PersonalTasks.Tasks.Sevices;

namespace PersonalTasks.Tasks.Controller
{
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
            TaskItem? taskItem = await _taskService.GetTask(id);
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
            TaskItem taskItem = _mapper.Map<TaskItem>(taskRequest);
            await _taskService.CreateTask(taskItem);
            return CreatedAtAction(nameof(GetTask), new { id = taskItem.Id }, taskItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest taskRequest)
        {
            TaskItem? taskItem = await _taskService.GetTask(id);
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
            TaskItem? taskItem = await _taskService.GetTask(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            await _taskService.DeleteTask(taskItem);
            return NoContent();

        }
    }
}