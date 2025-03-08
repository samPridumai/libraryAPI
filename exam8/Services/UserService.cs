using exam8.Interfaces;
using exam8.Models;

namespace exam8.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<int> RegisterUserAsync(User user)
    {
        return await userRepository.AddUserAsync(user);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userRepository.GetUserByEmailAsync(email);
    }
}