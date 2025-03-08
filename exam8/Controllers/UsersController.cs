using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("/api/users/")]
public class UsersController(IUserService userService, IBorrowedBookService borrowedBookService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        var userId = await userService.RegisterUserAsync(user);
        if (userId == 0)
            return BadRequest(new { message = "ошибка при регистрации" });
        
        var createdUser = await userService.GetUserByEmailAsync(user.Email);
        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email}, createdUser);
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound(new { message = "пользователь не найден" });

        return Ok(user);
    }

    [HttpGet("{email}/books")]
    public async Task<IActionResult> GetUserBooks(string email)
    {
        var books = await borrowedBookService.GetUserBooksAsync(email);
        return Ok(books);
    }

}