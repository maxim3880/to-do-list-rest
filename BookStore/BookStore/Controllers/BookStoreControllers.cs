using BookStore.BookStore.Data;
using BookStore.BookStore.Entities;
using BookStore.BookStore.Models;
using BookStore.BookStore.Models.Requests;
using BookStore.BookStore.Models.Responses;
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
        var books = new BookInfo(){ISBN = request.ISBN,Title = request.Title,SubTitle= request.SubTitle,Author = request.Author,PublishDate = request.PublishDate,Publisher = request.Publisher,Pages = request.Pages,Description=request.Description,Website= request.Website};
        var response = new BookResponse(){ISBN = books.ISBN, Title = books.Title, SubTitle = books.SubTitle, Author = books.Author, PublishDate = books.PublishDate, Publisher = books.Publisher, Pages = books.Pages , Description = books.Description, Website = books.Website};
        _context.BookInfos.Add(books);
        _context.SaveChanges();
        return Created("", response);
    }

    [HttpGet, Route("/BookStore/v1/Books")]
    public IActionResult ListBooks()
    {
        var book = _context.BookInfos.ToList();
        var respons = book.Select(b => new BookResponse() // creates a new Book Response object for each record from BookInfos, copying the required fields.
        {
            ISBN = b.ISBN, Title = b.Title, SubTitle = b.SubTitle, Author = b.Author, PublishDate = b.PublishDate, Publisher = b.Publisher, Pages = b.Pages , Description = b.Description, Website = b.Website
        }).ToList();
        return Ok(respons);
    }
    [HttpGet,Route("/BookStore/v1/Book/{ISBN}")]
    public IActionResult GetBookId(string isbn)
    {
        var books = _context.BookInfos.FirstOrDefault(x => x.ISBN == isbn);
        if (books == null) return NotFound(new { error = "ISBN not found" });
        _context.SaveChanges();
        var response = new BookResponse(){ISBN = books.ISBN, Title = books.Title, SubTitle = books.SubTitle, Author = books.Author, PublishDate = books.PublishDate, Publisher = books.Publisher, Pages = books.Pages , Description = books.Description, Website = books.Website};
        return Ok(response);
    }

    [HttpPost, Route("/BookStore/v1/Books")]
    public IActionResult CreatedUserAndISBN([FromBody] AddListOfBooks request)
    {
        var books = new AddListOfBooks(){UserId = request.UserId,ISBN = request.ISBN};
        var response = new AddListOfBooks(){UserId = books.UserId,ISBN = books.ISBN};
        _context.Add(response);
        _context.SaveChanges();
        return Created("", response);
        
    }

    [HttpPut, Route("/BookStore/v1/Book/{ISBN}")]
    public IActionResult UpdateBookByIsbn([FromBody] ReplaceIsbn update,string? isbn)
    {
        var book = _context.AddListOfBooks.FirstOrDefault(t => t.ISBN == isbn);
        if (book == null) return NotFound(new { error = "ISBN not found" });
        book.ISBN = update.ISBN;
        _context.SaveChanges();
        var response = new ReplaceIsbn(){ISBN = book.ISBN,UserId = book.UserId};
        return Accepted(response);
    }

    [HttpDelete, Route("/BookStore/v1/Books")]
    public IActionResult DeleteAllBookw(int userid)
    {
        // Retrieve all records where UserId matches the given userid
        var user = _context.AddListOfBooks.Where(t => t.UserId == userid).ToList();
        // If no records found, return 404 Not Found
        if (!user.Any()) return NotFound(new { error = "UserId not found" });
        // Remove all found records from the database context
        _context.RemoveRange(user);
        _context.SaveChanges();
        var response = new BooksResult() {UserId = userid };
        return Ok(response);
    }

    [HttpDelete, Route("/BookStore/v1/Book")]
    public IActionResult DeleteOneBook(string isbn)
    {
        var  book = _context.AddListOfBooks.FirstOrDefault(t => t.ISBN == isbn);
        if (book == null) return NotFound(new { error = "Book with this ISBN not found" });
        _context.Remove(book);
        _context.SaveChanges();
        return Ok(new { message = $"Book with ISBN {isbn} deleted" });
    }
}