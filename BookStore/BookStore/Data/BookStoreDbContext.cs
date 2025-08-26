using BookStore.BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BookStore.Data;

public  class BookStoreDbContext : DbContext, IBookStoreDbContext
{
    // The DbSet property is a collection of objects that maps to a specific table in the database
    public DbSet<BookInfo> BookInfos { get; set; } = null!;
    public DbSet<AddListOfBooks> AddListOfBooks { get; set; } = null!;
   

    public void RemoveRange(List<AddListOfBooks> user)
    {
        base.RemoveRange(user);
    }

    void IBookStoreDbContext.SaveChanges()
    {
        base.SaveChanges();
    }
    
   
    // In the constructor of the Contact DbContext class, the data context settings will be passed through the options parameter
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
        Database.EnsureCreated(); // create database on first access
    }
}