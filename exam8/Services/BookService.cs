using exam8.Interfaces;
using exam8.Models;

namespace exam8.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<int> AddBookAsync(Book book)
    {
        return await bookRepository.AddBookAsync(book);
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(int page, int pageSize)
    {
        return await bookRepository.GetBooksAsync(page, pageSize);
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await bookRepository.GetBookByIdAsync(id);
    }

    public async Task<bool> UpdateBookAsync(int id, Book book)
    {
        return await bookRepository.UpdateBookAsync(id, book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await bookRepository.DeleteBookAsync(id);
    }
}