using exam8.Models;

namespace exam8.Interfaces;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user);
    Task<User> GetUserByEmailAsync(string Email);
}