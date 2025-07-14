using Microsoft.EntityFrameworkCore;
using Tasks.Task.Data;

namespace Tasks;

public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // Создаем строитель приложения

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection"); // Получаем строку подключения из файла конфигурации appsettings.json

            builder.Services.AddDbContext<TaskDbContext>(options => options.UseSqlServer(connection)); // Регистрируем сервис базы данных (ContactDbContext) в контейнере зависимостей

            builder.Services.AddControllers(); // Добавляем сервис контроллеров

            var app = builder.Build(); 

            app.MapControllers(); // Указываем приложению, что нужно автоматически искать и подключать контроллеры по их маршрутам

            app.Run();
        }
    }
