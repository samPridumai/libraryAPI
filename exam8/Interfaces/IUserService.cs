using exam8.Models;

namespace exam8.Interfaces;

public interface IUserService
{
    Task<int> RegisterUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
}