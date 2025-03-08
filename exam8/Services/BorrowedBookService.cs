using exam8.Interfaces;
using exam8.Models;

namespace exam8.Services;

public class BorrowedBookService(
    IBorrowedBookRepository borrowedBookRepository,
    IUserRepository userRepository,
    IBookRepository bookRepository)
    : IBorrowedBookService
{

    public async Task<bool> BorrowBookAsync(int id, string userEmail)
    {
        var book = await bookRepository.GetBookByIdAsync(id);
        if (book == null || book.Status == "borrowed")
        {
            return false;
        }
        
        var user = await userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return false;
        }
        
        return await borrowedBookRepository.BorrowBookAsync(id, userEmail);
    }

    public async Task<bool> ReturnBookAsync(int id, string userEmail)
    {
        return await borrowedBookRepository.ReturnBookAsync(id, userEmail);
    }

    public async Task<IEnumerable<BorrowedBook>> GetBorrowedBooksAsync()
    {
        return await borrowedBookRepository.GetBorrowedBooksAsync();
    }

    public async Task<IEnumerable<BorrowedBook>> GetBooksAsync(string userEmail)
    {
        return await borrowedBookRepository.GetUserBookAsync(userEmail);
    }
}