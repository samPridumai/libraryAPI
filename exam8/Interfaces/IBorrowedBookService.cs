using exam8.Models;

namespace exam8.Interfaces;

public interface IBorrowedBookService
{
    Task<bool> BorrowBookAsync(int id, string userEmail);
    Task<bool> ReturnBookAsync(int id, string userEmail);
    Task<IEnumerable<BorrowedBook>> GetBorrowedBooksAsync();
    Task<IEnumerable<BorrowedBook>> GetBooksAsync(string userEmail);
    Task<IEnumerable<BorrowedBook>> GetUserBooksAsync(string email);
}