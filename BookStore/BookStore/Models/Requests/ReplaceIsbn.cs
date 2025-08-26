namespace BookStore.BookStore.Models.Requests;

public class ReplaceIsbn
{
    public int? UserId { get; set; }
    public string? ISBN { get; set; }
}