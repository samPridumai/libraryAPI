using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookRepository bookRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Book>> Create(Book book)
}