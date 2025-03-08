using exam8.Interfaces;
using exam8.Models;

namespace exam8.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public Task<int> AddBookAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> GetBooksAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Book?> GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateBookAsync(int id, Book book)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteBookAsync(int id)
    {
        throw new NotImplementedException();
    }
}