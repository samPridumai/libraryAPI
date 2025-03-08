using System.Data;
using Dapper;
using exam8.Interfaces;
using exam8.Models;
using Microsoft.AspNetCore.Mvc;

namespace exam8.Repositories;

public class UserRepository(IDbConnection connection) : IUserRepository
{
    public async Task<int> AddUserAsync(User user)
    {
        var sql = """
                  INSERT INTO users
                      (firstname, lastname, email, phonenumber)
                  VALUES
                      (@firstname, @lastname, @email, @phonenumber)
                  RETURNING id;
                  """;
        
        return await connection.QuerySingleAsync<int>(sql, user);
    }

    public async Task<User> GetUserByEmailAsync(string Email)
    {
        var sql = "SELECT * FROM users WHERE email = @email";
        return await connection.QuerySingleAsync<User>(sql, new
        {
            email = Email
        });
    }
    
    public async Task<IEnumerable<BorrowedBook>> GetUserBooksAsync(string email)
    {

        var sql = """
                  SELECT b.id as BookId, b.title as BookTitle, b.author as BookAuthor, b.borrowedAt as BookBorrowedAt
                  FROM borrowedbooks as bb
                  JOIN books b ON bb.bookid = b.id
                  WHERE b.email = @Email;
                  """;
        return await connection.QueryAsync<BorrowedBook>(sql, new { Email = email });
    }
}