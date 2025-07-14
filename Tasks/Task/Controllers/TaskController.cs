using Microsoft.AspNetCore.Mvc;
using Tasks.Task.Data;
using Tasks.Task.Models;

namespace Tasks.Task.Controllers;

public class TaskController : ControllerBase
{
    private readonly TaskDbContext _context;

    public TaskController(TaskDbContext context)
    {
        // Когда приложение запускает твой контроллер, оно само передаёт сюда готовый доступ к базе (context).
        _context = context;
    }
    [HttpPost, Route("/task")]
    public IActionResult GetContacts([FromBody]TaskInfo task)
    {
        // Добавляем новый контакт в базу через контекст
        _context.TaskInfos.Add(task);
        // Сохраняем изменения в базе данных
        _context.SaveChanges();
        return Ok(task);
    }
    
    [HttpGet, Route("/task")]
    public IActionResult Index()
    {
        
        var task = _context.TaskInfos.ToList();

        string result = "";

        foreach (var tasks in task)
        {
            result += $"Description: {tasks.Description} Completed: {tasks.Completed}\n";
        }
        return Ok(task);
    }

    [HttpGet, Route("/task/{id}")]
    public IActionResult GetTask([FromBody] TaskInfo task, int id)
    {
        var tasks = _context.TaskInfos.FirstOrDefault(t => t.id == id);
        if (tasks == null) return NotFound(new { error = "Contact not found" });
        return Ok(tasks);
    }
    
    [HttpGet("tasks/{completed}")]
    public IActionResult GetCompleted(string completed)
    {
        // Проверяем, что введено "true" или "false"
        if (completed != "true" && completed != "false")
        {
            return BadRequest(new { error = "Invalid value. Use 'true' or 'false'." });
        }
        // Фильтруем задачи по полю completed
        var tasks = _context.TaskInfos.Where(p => p.Completed == completed).ToList();
        if (tasks.Count == 0)
        {
            return NotFound(new { error = "No tasks found." });
        }
        return Ok(tasks);
    }

    [HttpGet("tasks")]
    public IActionResult GetTasksWithLimitAndSkip(int skip = 0, int limit = 3)
    {
        if (limit <= 0)
            return BadRequest(new { error = "Limit must be greater than 0." });
        // Считаем общее количество задач в базе данных
        var totalTasks = _context.TaskInfos.Count();
        // Получаем задачи с учётом пропуска и лимита:
        var tasks = _context.TaskInfos
            .Skip(skip)
            .Take(limit)
            .ToList();
        
        return Ok(new
        {
            skip,
            limit,
            totalTasks,
            tasks
        });
    }

    [HttpPut, Route("/task/{id}")]
    public IActionResult UpdateTask([FromBody] TaskInfo task, int id)
    {
        var tasks = _context.TaskInfos.FirstOrDefault(t => t.id == id);
        if (tasks == null) return NotFound(new { error = "Contact not found" });
        tasks.Completed = task.Completed;
        _context.SaveChanges();
        return Ok(task);
    }
    
    [HttpDelete, Route("/task/{id}")]
    public IActionResult DeleteTask([FromBody] TaskInfo task, int id)
    {
        var tasks = _context.TaskInfos.FirstOrDefault(t => t.id == id);
        if (tasks == null) return NotFound(new { error = "Contact not found" });
        _context.Remove(tasks);
        _context.SaveChanges();
        return Ok(task);
    }
}