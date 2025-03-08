using System.Data;
using Dapper;
using exam8.Interfaces;
using exam8.Models;

namespace exam8.Repositories;

public class BookRepository(IDbConnection connection) : IBookRepository
{
    public async Task<int> AddBookAsync(Book book)
    {
        var sql = $"""
                   INSERT INTO Books
                   (
                   {nameof(Book.Title)}, 
                   {nameof(Book.Author)}, 
                   {nameof(Book.CoverImageUrl)}, 
                   {nameof(Book.PublicationYear)}, 
                   {nameof(Book.Description)},
                   {nameof(Book.Status)},
                   {nameof(Book.CreatedAt)})
                   VALUES
                   (
                   @{nameof(Book.Title)}, 
                   @{nameof(Book.Author)}, 
                   @{nameof(Book.CoverImageUrl)}, 
                   @{nameof(Book.PublicationYear)}, 
                   @{nameof(Book.Description)},
                   @{nameof(Book.Status)},
                   @{nameof(Book.CreatedAt)})
                   RETURNING id;
                   """;

        return await connection.QuerySingleAsync<int>(sql, book);


    }

    public async Task<IEnumerable<Book>> GetBooksAsync(int page, int pageSize)
    {
        var sql = """
                    SELECT * FROM Books
                    ORDER BY {nameof(Book.CreatedAt)} DESC
                    LIMIT @PageSize OFFSET @Offset";
                    """;
        return await connection.QueryAsync<Book>(sql, new { PageSize = pageSize, Offset = (page - 1) * pageSize });
    }

    public async Task<Book?> GetBookAsyncByIdAsync(int id)
    {
        var sql = "SELECT * FROM Books WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
    }

    public async Task<bool> UpdateBookAsync(int id, Book book)
    {
        var sql = """
                  UPDATE Books
                  Set @{nameof(Book.Title)} = @{nameof(Book.Title)},
                      @{nameof(Book.Author)} = @{nameof(Book.Author)},
                      @{nameof(Book.CoverImageUrl)} = @{nameof(Book.CoverImageUrl)},
                      @{nameof(Book.PublicationYear)} = @{nameof(Book.PublicationYear)},
                      @{nameof(Book.Description)} = @{nameof(Book.Description)}
                      WHERE Id = @Id";"
                  """;
        return await connection.ExecuteAsync(sql, book) > 0;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var sql = "DELETE FROM Books WHERE {nameof(Book.Id)} = @{nameof(Book.Id)}";
        return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}