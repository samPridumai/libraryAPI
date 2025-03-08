using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("api/books")]
public class BorrowedBooksController(IBorrowedBookService borrowedBookService, IUserService userService, IBookService bookService) : ControllerBase
{
    [HttpPost("{id}/borrow")]
    public async Task<IActionResult> BorrowBook(int id, [FromBody] BorrowRequest request)
    {
        try
        {
            var user = await userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound(new { noUserMessage = "Пользователь не найден" });
            }

            var book = await bookService.GetBookByIdAsync(id);
            if (book is null)
            {
                return NotFound(new { noBookMessage = "Книга не найдена" });
            }

            if (book.Status != "available")
            {
                return BadRequest(new { bookBorrowedessage = "Книга уже занята" });
            }

            bool success = await borrowedBookService.BorrowBookAsync(id, user.Email);
            if (!success)
            {
                return BadRequest( new { errorMessage = "Не удалось выдать книгу" });
            }

            return Ok(new { message = "Книга успешно выдана" });
        }
        catch (Exception e)
        {
            return BadRequest( new { exceptionMessage = "Не удалось выдать книгу", e.Message });
        }
    }
}