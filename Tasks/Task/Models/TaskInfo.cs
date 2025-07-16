using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Task.Models;

public class TaskInfo
{
    public int id { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}