using exam8.Models;

namespace exam8.Interfaces;

public interface IBookRepository
{
    Task<int> AddBookAsync(Book book);
    Task<IEnumerable<Book>> GetBooksAsync(int page, int pageSize);
    Task<Book?> GetBookAsyncByIdAsync(int id);
    Task<bool> UpdateBookAsync(int id,Book book);
    Task<bool> DeleteBookAsync(int id);
}