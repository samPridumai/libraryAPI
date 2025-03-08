using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookService bookService, IBorrowedBookService borrowedBookService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book)
    {
        var bookId = await bookService.AddBookAsync(book);
        if (bookId == 0)
        {
            return BadRequest(new { message = "неверные данные" });
        }
        
        var createdBook = await bookService.GetBookByIdAsync(bookId);
        return CreatedAtAction(nameof(GetBookById), new {id = bookId}, createdBook);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
    {
        var books = await bookService.GetBooksAsync(page, pageSize);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound(new { message = "не найдено" });
        }
        return Ok(book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        var updatedBook = await bookService.UpdateBookAsync(id, book);
        if (!updatedBook)
        {
            return NotFound(new { message = "не найдено" });
        }
        return Ok(new { message = "Книга успешно обновлена" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var deletedBook = await bookService.DeleteBookAsync(id);
        if (!deletedBook)
            return NotFound(new { message = "не найдено" });

        return Ok(new { message = "Книга успешно удалена" });
    }

    [HttpPost("{id}/return")]
    public async Task<ActionResult<Book>> ReturnBook(int id, [FromBody] BorrowRequest request)
    {
        var email = request.Email;
        bool success = await borrowedBookService.ReturnBookAsync(id, email);
        if (!success)
        {
            return BadRequest(new { returnErrorMessage = "ошибка" });
        }

        return Ok(new { message = "Книга успешно возвращена" });
    }
}
