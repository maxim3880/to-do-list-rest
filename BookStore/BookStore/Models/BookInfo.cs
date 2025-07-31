using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BookStore.Models;

public class BookInfo
{
    [Key]
    public string? ISBN { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? Author { get; set; }
    public string? PublishDate { get; set; }
    public string? Publisher { get; set; }
    public int Pages { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
}