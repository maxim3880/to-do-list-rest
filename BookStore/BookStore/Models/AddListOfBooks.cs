using System.ComponentModel.DataAnnotations;

namespace BookStore.BookStore.Models;

public class AddListOfBooks
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ISBN { get; set; }
}