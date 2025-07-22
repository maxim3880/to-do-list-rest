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
        // When the application starts your controller, it itself passes ready access to the database (context) here.
        _context = context;
    }
    
    [HttpPost]
    public IActionResult CreatedTask([FromBody] CreateTaskRequest request)
    {
        var task = new TaskInfo();
        task.Description = request.Description; // сделали 
        // Add a new contact to the database via context
        _context.TaskInfos.Add(task);
        // Save changes to the database
        _context.SaveChanges();
        var response = new TaskResponse() { Id = task.Id, Description = task.Description, Completed = task.Completed };
        return Created("", response);
    }
    [HttpGet]
    public IActionResult GetTasks( int? skip, int? limit, bool? completed)
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
       var response = new List<TaskResponse>();
       foreach (var task in tasks)
       {
           var respons = new TaskResponse() { Id = task.Id, Description = task.Description, Completed = task.Completed };
           response.Add(respons);
       }
       return Ok(response);
    }
    
    [HttpGet, Route("{id}")]
    public IActionResult GetTaskById(int id)
    {
        var tasks = _context.TaskInfos.FirstOrDefault(x => x.Id == id);
        if (tasks == null) return NotFound(new { message = "Contact not found" });
        _context.SaveChanges();
        var response = new TaskResponse() { Id = tasks.Id, Description = tasks.Description, Completed = tasks.Completed };
        return Ok(response);
    }
    [HttpPut, Route("{id}")]
    public IActionResult UpdateTaskById([FromBody]UpdateTaskRequest update, int id)
    {
        var task = _context.TaskInfos.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound(new { error = "Contact not found" });
        task.Completed = update.Completed;
        _context.SaveChanges();
        var response = new TaskResponse() {  Id = task.Id, Description = task.Description, Completed = task.Completed };
        return Accepted(response);
    }
    
    [HttpDelete, Route("{id}")]
    public IActionResult DeleteTaskById(int id)
    {
        var task = _context.TaskInfos.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound(new { error = "Contact not found" });
        _context.Remove(task);
        _context.SaveChanges();
        var response = new TaskResponse() { Id = task.Id, Description = task.Description, Completed = task.Completed };
        return Ok(response);
    }
}

