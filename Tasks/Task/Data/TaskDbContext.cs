using Microsoft.EntityFrameworkCore;
using Tasks.Task.Models;

namespace Tasks.Task.Data;

public  class TaskDbContext : DbContext
{
    // Свойство DbSet представляет собой коллекцию объектов, которая сопоставляется с определенной таблицей в базе данных
    public DbSet<TaskInfo> TaskInfos { get; set; } = null!;
    // В конструкторе класса ContactDbContext через параметр options будут передаваться настройки контекста данных
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
        Database.EnsureCreated(); // создаем базу данных при первом обращении
    }

}