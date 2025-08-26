using System.ComponentModel.DataAnnotations;

namespace BookStore.BookStore.Entities;

public class AddListOfBooks
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ISBN { get; set; }
}          