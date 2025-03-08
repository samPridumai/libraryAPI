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
            return BadRequest(new { message = "Invalid book data" });
        }

        var bookId = await bookRepository.AddBookAsync(book);
        var createdBook = await bookRepository.GetBookAsyncByIdAsync(bookId);
        
        return CreatedAtAction(nameof(GetBookById), new { id = bookId }, createdBook);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest(new { message = "Page and pageSize must be greater than zero" });
        }

        var books = await bookRepository.GetBooksAsync(page, pageSize);
        var totalCount = await bookRepository.GetTotalBooksCountAsync(); 

        if (!books.Any())
        {
            return NotFound(_bookNotFound);
        }

        return Ok(new
        {
            page,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
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
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        if (book == null)
        {
            return BadRequest(new { message = "Invalid book data" });
        }

        var exists = await bookRepository.GetBookAsyncByIdAsync(id);
        if (exists == null)
        {
            return NotFound(_bookNotFound);
        }

        var updated = await bookRepository.UpdateBookAsync(id, book);
        if (!updated)
        {
            return StatusCode(500, new { message = "Failed to update book" });
        }

        return NoContent(); // 204 No Content
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var exists = await bookRepository.GetBookAsyncByIdAsync(id);
        if (exists == null)
        {
            return NotFound(_bookNotFound);
        }

        var deleted = await bookRepository.DeleteBookAsync(id);
        if (!deleted)
        {
            return StatusCode(500, new { message = "Failed to delete book" });
        }

        return NoContent(); // 204 No Content
    }
}
