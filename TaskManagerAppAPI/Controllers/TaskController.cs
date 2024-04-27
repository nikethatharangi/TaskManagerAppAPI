using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerAppAPI.Models;
using Task = TaskManagerAppAPI.Models.Task;

namespace TaskManagerAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        public readonly TaskDbContext _db;

        public TaskController(TaskDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTasks()
        {
            var tasks = await _db.Tasks.ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] Task task)
        {
            await  _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Task updatedTask)
        {
            var existingTask = await _db.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;

            _db.Tasks.Update(existingTask);
            await _db.SaveChangesAsync();
            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
