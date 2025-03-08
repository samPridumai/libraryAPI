using System.Data;
using Dapper;
using exam8.Interfaces;
using exam8.Models;

namespace exam8.Repositories;

public class BorrowedBookRepository(IDbConnection connection) : IBorrowedBookRepository
{
    public async Task<bool> BorrowBookAsync(int bookId, string userEmail)
    {
        var sql = """
                  INSERT INTO borrowed_book
                  (book_id, user_email, borrowed_at)
                  VALUES (@book_id, @user_email, NOW());
                  """;
        return await connection.ExecuteAsync(sql, new
        {
            book_id = bookId, user_email = userEmail
        }) > 0;
    }

    public async Task<bool> ReturnBookAsync(int bookId, string userEmail)
    {
        var deleteSql = "DELETE FROM borrowed_book WHERE book_id = @book_id AND user_email = @user_email;";
        int updated = await connection.ExecuteAsync(deleteSql, new
        {
            book_id = bookId, user_email = userEmail
        });

        if (updated > 0)
        {
            var updateSql = "UPDATE books SET status = 'available' WHERE id = @BookId;";
            await connection.ExecuteAsync(updateSql, new
            {
                BookId = bookId
            });
        }
        return updated > 0;
    }
    
    public async Task<bool> IsBorrowedBookAsync(int bookId, string userEmail)
    {
        string sql = "SELECT COUNT(*) FROM borrowed_books WHERE book_id = @BookId;";
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            BookId = bookId, user_email = userEmail
        }) > 0;
    }

    public Task<IEnumerable<BorrowedBook>> GetBorrowedBooksAsync()
    {
        var sql = "SELECT * FROM borrowed_books";
        return connection.QueryAsync<BorrowedBook>(sql);
    }

    public async Task<IEnumerable<BorrowedBook>> GetUserBookAsync(string userEmail)
    {
        var sql = """
                  SELECT * FROM books b 
                           JOIN borrowed_books bb ON b.id = bb.book_id
                           WHERE bb.user_email = @UserEmail";
                  """;
        return await connection.QueryAsync<BorrowedBook>(sql, new { UserEmail = userEmail }); 
    }
}