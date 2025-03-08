using System.Data;
using Dapper;
using exam8.Interfaces;
using exam8.Models;

namespace exam8.Repositories;

public class BorrowedBookRepository(IDbConnection connection) : IBorrowedBookRepository
{
    public async Task<bool> BorrowBookAsync(int bookId, string userEmail)
    {
        var userId = await connection.QueryFirstOrDefaultAsync<int?>(
            "SELECT id FROM users WHERE email = @Email", new { Email = userEmail });

        if (userId == null)
            return false;

        var sqlInsert = """
                        INSERT INTO borrowedbooks (bookid, userid, borrowedat)
                        VALUES (@BookId, @UserId, NOW());
                        """;

        var sqlUpdate = """
                        UPDATE books SET status = 'borrowed' WHERE id = @BookId;
                        """;

        int affectedRows = await connection.ExecuteAsync(sqlInsert, new { BookId = bookId, UserId = userId });

        if (affectedRows > 0)
        {
            await connection.ExecuteAsync(sqlUpdate, new { BookId = bookId });
        }

        return affectedRows > 0;
    }

    public async Task<bool> ReturnBookAsync(int bookId, string userEmail)
    {
        var userId = await connection.QueryFirstOrDefaultAsync<int?>(
            "SELECT id FROM users WHERE email = @Email", new { Email = userEmail });

        if (userId == null)
            return false;

        var sqlUpdateBorrowed = """
                                UPDATE borrowedbooks
                                SET returnedat = NOW()
                                WHERE bookid = @BookId AND userid = @UserId AND returnedat IS NULL;
                                """;

        var sqlUpdateBook = """
                            UPDATE books
                            SET status = 'available'
                            WHERE id = @BookId;
                            """;

        int affectedRows = await connection.ExecuteAsync(sqlUpdateBorrowed, new { BookId = bookId, UserId = userId });

        if (affectedRows > 0)
        {
            await connection.ExecuteAsync(sqlUpdateBook, new { BookId = bookId });
        }

        return affectedRows > 0;
    }


    public async Task<bool> IsBorrowedBookAsync(int bookId, string userEmail)
    {
        var userId = await connection.QueryFirstOrDefaultAsync<int?>(
            "SELECT id FROM users WHERE email = @Email", new { Email = userEmail });
        
        if (userId == null)
            return false;

        var sql = """
                  SELECT COUNT(*) FROM borrowedbooks
                  WHERE bookid = @BookId AND userid = @UserId AND returnedat IS NULL;
                  """;
        
        return await connection.ExecuteScalarAsync<int>(sql, new { BookId = bookId, UserId = userId }) > 0;
    }

    public async Task<IEnumerable<BorrowedBook>> GetBorrowedBooksAsync()
    {
        var sql = "SELECT * FROM borrowedbooks WHERE returnedat IS NULL";
        return await connection.QueryAsync<BorrowedBook>(sql);
    }

    public async Task<IEnumerable<BorrowedBook>> GetUserBookAsync(string userEmail)
    {
        var userId = await connection.QueryFirstOrDefaultAsync<int?>(
            "SELECT id FROM users WHERE email = @Email", new { Email = userEmail });
        
        if (userId == null)
            return Enumerable.Empty<BorrowedBook>();

        var sql = "SELECT * FROM borrowedbooks WHERE userid = @UserId AND returnedat IS NULL";
        return await connection.QueryAsync<BorrowedBook>(sql, new { UserId = userId });
    }
}
