using BookStore.BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Подключаем БД
        builder.Services.AddDbContext<BookStoreDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Регистрируем интерфейс поверх реализации
        builder.Services.AddScoped<IBookStoreDbContext>(provider => 
            provider.GetRequiredService<BookStoreDbContext>());

        // Регистрируем контроллеры
        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}