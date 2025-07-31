using BookStore.BookStore.Data;
using BookStore.BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BookStore.Controllers;

public class BookStoreControllers : ControllerBase
{
    private readonly BookStoreDbContext _context;

    public BookStoreControllers(BookStoreDbContext context)
    {
        _context = context;
    }
    [HttpPost, Route("/BookStore/v1/Book")]
    public IActionResult CreatedBook([FromBody] BookResponse request)
    {
        var books = new BookInfo();
        books.ISBN = request.ISBN;
        books.Title = request.Title;
        books.SubTitle = request.SubTitle;
        books.Author = request.Author;
        books.PublishDate = request.PublishDate;
        books.Publisher = request.Publisher;
        books.Pages = request.Pages;
        books.Description = request.Description;
        books.Website = request.Website;
        var response = new BookResponse(){ISBN = books.ISBN, Title = books.Title, SubTitle = books.SubTitle, Author = books.Author, PublishDate = books.PublishDate, Publisher = books.Publisher, Pages = books.Pages , Description = books.Description, Website = books.Website};
        _context.BookInfos.Add(books);
        _context.SaveChanges();
        return Created("", response);
    }

    [HttpGet, Route("/BookStore/v1/Books")]
    public IActionResult ListBooks()
    {
        var book = _context.BookInfos.ToList();
        var respons = new List<BookResponse>();
        foreach (var books in book)
        {
            var response = new BookResponse(){ISBN = books.ISBN, Title = books.Title, SubTitle = books.SubTitle, Author = books.Author, PublishDate = books.PublishDate, Publisher = books.Publisher, Pages = books.Pages , Description = books.Description, Website = books.Website};
            respons.Add(response);
        }
        return Ok(respons);
    }
    [HttpGet,Route("/BookStore/v1/Book/{ISBN}")]
    public IActionResult GetBookId(string isbn)
    {
        var books = _context.BookInfos.FirstOrDefault(x => x.ISBN == isbn);
        if (books == null) return NotFound(new { error = "Contact not found" });
        _context.SaveChanges();
        var response = new BookResponse(){ISBN = books.ISBN, Title = books.Title, SubTitle = books.SubTitle, Author = books.Author, PublishDate = books.PublishDate, Publisher = books.Publisher, Pages = books.Pages , Description = books.Description, Website = books.Website};
        return Ok(response);
    }

    [HttpPost, Route("/BookStore/v1/Books")]
    public IActionResult CreatedUserAndISBN([FromBody] AddListOfBooks request)
    {
        var books = new AddListOfBooks();
        books.UserId = request.UserId;
        books.ISBN = request.ISBN;
        var response = new AddListOfBooks(){UserId = books.UserId,ISBN = books.ISBN};
        _context.Add(response);
        _context.SaveChanges();
        return Created("", response);
        
    }

    [HttpPut, Route("/BookStore/v1/Book/{ISBN}")]
    public IActionResult UpdateBookByIsbn([FromBody] ReplaceIsbn update,string? isbn)
    {
        var book = _context.AddListOfBooks.FirstOrDefault(t => t.ISBN == isbn);
        if (book == null) return NotFound(new { error = "Contact not found" });
        book.ISBN = update.ISBN;
        _context.SaveChanges();
        var response = new ReplaceIsbn(){ISBN = book.ISBN,UserId = book.UserId};
        return Accepted(response);
    }

    [HttpDelete, Route("/BookStore/v1/Books")]
    public IActionResult DeleteUserAndISBN(int userid)
    {
        // Retrieve all records where UserId matches the given userid
        var user = _context.AddListOfBooks.Where(t => t.UserId == userid).ToList();
        // If no records found, return 404 Not Found
        if (!user.Any()) return NotFound(new { error = "Contact not found" });
        // Remove all found records from the database context
        _context.RemoveRange(user);
        _context.SaveChanges();
        var response = new BooksResult() {UserId = userid };
        return Ok(response);
    }

    [HttpDelete, Route("/BookStore/v1/Book")]
    public IActionResult Delete()
    {
        
    }
}