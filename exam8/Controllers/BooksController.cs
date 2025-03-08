using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookRepository bookRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book)
    {
        if (book == null)
        {
            return BadRequest(new { message = "неверные данные" });
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
            return BadRequest(new { message = "неверные данные" });
        }

        var books = await bookRepository.GetBooksAsync(page, pageSize);
        var totalCount = await bookRepository.GetTotalBooksCountAsync(); 

        if (!books.Any())
        {
            return NotFound(new { message = "не найдено" });
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
            return NotFound(new { message = "не найдено" });
        }
        return Ok(book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        if (book == null)
        {
            return BadRequest(new { message = "неверные данные" });
        }

        var exists = await bookRepository.GetBookAsyncByIdAsync(id);
        if (exists == null)
        {
            return NotFound(new { message = "не найдено" });
        }

        var updated = await bookRepository.UpdateBookAsync(id, book);
        if (!updated)
        {
            return BadRequest(new { message = "неверные данные" });
        }

        return Ok(new { message = "Книга успешно обновлена" }); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var exists = await bookRepository.GetBookAsyncByIdAsync(id);
        if (exists == null)
        {
            return NotFound(new { message = "не найдено" });
        }

        var deleted = await bookRepository.DeleteBookAsync(id);
        if (!deleted)
        {
            return BadRequest(new { message = "неверные данные" });
        }

        return Ok(new { message = "Книга успешно удалена" });
    }
}
