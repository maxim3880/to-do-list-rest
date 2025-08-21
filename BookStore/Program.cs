using BookStore.BookStore.Data;
using Ingenico.Connect.Sdk.DefaultImpl;
using Microsoft.EntityFrameworkCore;

namespace BookStore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);// Create an application builder

        string? connection = builder.Configuration.GetConnectionString(nameof(DefaultConnection)); // Get the connection string from the appsettings.json configuration file

        builder.Services.AddDbContext<BookStoreDbContext, BookStoreDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register the database service (Contact DbContext) in the dependency container

        builder.Services.AddControllers(); // Add controller service

        var app = builder.Build(); 

        app.MapControllers(); // Tell the application to automatically search for and connect controllers by their routes

        app.Run();
    }
}