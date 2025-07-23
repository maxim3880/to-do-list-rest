using Microsoft.EntityFrameworkCore;
using Tasks.Task.Models;

namespace Tasks.Task.Data;

public class TaskDbContext : DbContext // changed class to interface
{
    // The DbSet property is a collection of objects that maps to a specific table in the database
    public DbSet<TaskInfo> TaskInfos { get; set; } = null!;
    // In the constructor of the Contact DbContext class, the data context settings will be passed through the options parameter
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
        Database.EnsureCreated(); // create database on first access
    }

}