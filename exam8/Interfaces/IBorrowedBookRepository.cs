using exam8.Models;

namespace exam8.Interfaces;

public interface IBorrowedBookRepository
{
    Task<bool> BorrowBookAsync(int bookId, string userEmail);
    Task<bool> ReturnBookAsync(int bookId, string userEmail);
    Task<bool> IsBorrowedBookAsync(int bookId, string userEmail);
    Task<IEnumerable<BorrowedBook>> GetBorrowedBooksAsync();
    Task<IEnumerable<BorrowedBook>> GetUserBookAsync(string userEmail);
}