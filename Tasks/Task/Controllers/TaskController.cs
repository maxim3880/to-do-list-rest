using Microsoft.AspNetCore.Mvc;
using Tasks.Task.Data;
using Tasks.Task.Models;

namespace Tasks.Task.Controllers;

[Route("task")]
public class TaskController : ControllerBase
{
    private readonly TaskDbContext _context;

    public TaskController(TaskDbContext context)
    {
        // Когда приложение запускает твой контроллер, оно само передаёт сюда готовый доступ к базе (context).
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskInfo task)
    {
        // Добавляем новый контакт в базу через контекст
        _context.TaskInfos.Add(task);
        // Сохраняем изменения в базе данных
        _context.SaveChanges();
        return Ok(task);
    }

    [HttpGet]
    public IActionResult GetTasks(int? skip, int? limit, bool? completed)
    {
       var taskQuery = _context.TaskInfos.AsQueryable();
       if (completed.HasValue)
       {
           taskQuery = taskQuery.Where(t => t.Completed == completed);
       }

       if (limit.HasValue && limit.Value > 0)
       {
           taskQuery = taskQuery.Take(limit.Value);
       }

       if (skip.HasValue)
       {
           taskQuery = taskQuery.Skip(skip.Value);
       }
       var tasks = taskQuery.ToList();
       return Ok(tasks);
    }

    [HttpGet, Route("{id}")]
    public IActionResult GetTaskById(int id)
    {
        var task = _context.TaskInfos.FirstOrDefault(x => x.id == id);
        if (task == null) return NotFound(new { message = "Пользователь не найден" });
        return Accepted(task);
    }
    [HttpPut, Route("{id}")]
    public IActionResult UpdateTaskById([FromBody] TaskInfo tasks, int id)
    {
        var task = _context.TaskInfos.FirstOrDefault(t => t.id == id);
        if (task == null) return NotFound(new { error = "Contact not found" });
        task.Completed = tasks.Completed;
        _context.SaveChanges();
        return Accepted(task);
    }
    
    [HttpDelete, Route("{id}")]
    public IActionResult DeleteTaskById([FromBody] TaskInfo tasks, int id)
    {
        var task = _context.TaskInfos.FirstOrDefault(t => t.id == id);
        if (task == null) return NotFound(new { error = "Contact not found" });
        _context.Remove(task);
        _context.SaveChanges();
        return Ok(tasks);
    }
}

