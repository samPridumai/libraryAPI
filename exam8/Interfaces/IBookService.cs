using exam8.Models;

namespace exam8.Interfaces;

public interface IBookService
{
    Task<int> AddBookAsync(Book book);
    Task<IEnumerable<Book>> GetBooksAsync(int page, int pageSize);
    Task<Book?> GetBookByIdAsync(int id);
    Task<bool> UpdateBookAsync(int id, Book book);
    Task<bool> DeleteBookAsync(int id);
}