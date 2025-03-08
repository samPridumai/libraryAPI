using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookRepository bookRepository) : ControllerBase
{
    private readonly string _bookNotFound = "Book Not Found";
    
    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book)
    {
        if (book == null)
        {
            return BadRequest();
        }
        
        var bookId = await bookRepository.AddBookAsync(book);
        var createdBook = await bookRepository.GetBookAsyncByIdAsync(bookId);
        return Ok(createdBook);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
    {
        var books = await bookRepository.GetBooksAsync(page, pageSize);

        if (!books.Any())
        {
            return NotFound(_bookNotFound);
        }
        
        return Ok(new
        {
            page,
            totalPages = (int)Math.Ceiling((double)books.Count() / pageSize),
            books
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await bookRepository.GetBookAsyncByIdAsync(id);

        if (book == null)
        {
            return NotFound(_bookNotFound);
        }
        
        return Ok(book);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> UpdateBook(int id, [FromBody] Book book)
    {
        if (book == null) 
        {
            return BadRequest(new { message = "wrong data" });
        }
        
        var exists = await bookRepository.GetBookAsyncByIdAsync(id);
        if (exists == null)
        {
            return NotFound(_bookNotFound);
        }
        
        var updatedBook = await bookRepository.UpdateBookAsync(id, book);
        if (!updatedBook)
        {
            return StatusCode(500);
        }
        return Ok("Book updated succesfully");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Book>> DeleteBook(int id)
    {
        var exists = await bookRepository.GetBookAsyncByIdAsync(id);
        if (exists == null)
        {
            return NotFound(_bookNotFound);
        }
        var deletedBook = await bookRepository.DeleteBookAsync(id);
        if (!deletedBook)
        {
            return StatusCode(500);
        }
        return Ok("Book deleted succesfully");
    }
}