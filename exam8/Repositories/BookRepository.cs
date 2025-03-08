using System.Data;
using Dapper;
using exam8.Interfaces;
using exam8.Models;

namespace exam8.Repositories;

public class BookRepository(IDbConnection connection) : IBookRepository
{
    public async Task<int> AddBookAsync(Book book)
    {
        var sql = """
                   INSERT INTO Books
                   (Title, Author, CoverImageUrl, PublicationYear, Description, Status, CreatedAt)
                   VALUES
                   (@Title, @Author, @CoverImageUrl, @PublicationYear, @Description, @Status, @CreatedAt)
                   RETURNING id;
                   """;

        return await connection.QuerySingleAsync<int>(sql, book);
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(int page, int pageSize)
    {
        var sql = """
                  SELECT * FROM Books
                  ORDER BY CreatedAt DESC
                  LIMIT @PageSize OFFSET @Offset;
                  """;

        return await connection.QueryAsync<Book>(sql, new { PageSize = pageSize, Offset = (page - 1) * pageSize });
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        var sql = "SELECT * FROM Books WHERE Id = @Id;";
        return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
    }

    public async Task<bool> UpdateBookAsync(int id, Book book)
    {
        var sql = """
                  UPDATE Books
                  SET Title = @Title,
                      Author = @Author,
                      CoverImageUrl = @CoverImageUrl,
                      PublicationYear = @PublicationYear,
                      Description = @Description,
                      Status = @Status
                  WHERE Id = @Id;
                  """;

        var parameters = new
        {
            book.Title,
            book.Author,
            book.CoverImageUrl,
            book.PublicationYear,
            book.Description,
            book.Status,
            Id = id
        };

        return await connection.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var sql = "DELETE FROM Books WHERE Id = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }

    public async Task<int> GetTotalBooksCountAsync()
    {
        var sql = "SELECT COUNT(*) FROM Books";
        return await connection.ExecuteScalarAsync<int>(sql);
    }
}
