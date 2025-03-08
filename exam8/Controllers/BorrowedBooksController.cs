using exam8.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("api/books")]
public class BorrowedBooksController(IBorrowedBookService borrowedBookService, IUserService userService, IBookService bookService) : ControllerBase
{
    [HttpPost("{id}/borrow")]
    public async Task<ActionResult> BorrowBookBy(int id, [FromBody] string userEmail)
    {
        var user = await userService.GetUserByEmailAsync(userEmail);
        if (user is null)
            return NotFound(new { message = "пользователь не найден" });
        
        var book = await bookService.GetBookByIdAsync(id);
        if (book is null)
            return NotFound(new { message = "книга не найдена" });

        if (book.Status != "available")
            return BadRequest(new { message = "книга уже выдана" });
        
        await borrowedBookService.BorrowBookAsync(book.Id, userEmail);
        return Ok( new { message = "книга успешно выдана" });
    }
}