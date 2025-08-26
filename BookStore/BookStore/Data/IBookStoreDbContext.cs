using BookStore.BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BookStore.Data;

public interface IBookStoreDbContext
{
    public DbSet<BookInfo> BookInfos { get; set; }
    public DbSet<AddListOfBooks> AddListOfBooks { get; set; }
    void RemoveRange(List<AddListOfBooks> user);
    void SaveChanges();
}