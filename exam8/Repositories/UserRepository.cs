using System.Data;
using Dapper;
using exam8.Interfaces;
using exam8.Models;

namespace exam8.Repositories;

public class UserRepository(IDbConnection connection) : IUserRepository
{
    public async Task<int> AddUserAsync(User user)
    {
        var sql = """
                  INSERT INTO users
                      (first_name, last_name, email, phone_number)
                  VALUES
                      (@first_name, @last_name, @email, @phone_number)
                  RETURNING id;
                  """;
        return await connection.QuerySingleAsync<int>(sql, user);
    }

    public async Task<User> GetUserByEmailAsync(string Email)
    {
        var sql = "SELECT * FROM users WHERE email = @email";
        return await connection.QuerySingleAsync<User>(sql, new { email = Email });
    }
}