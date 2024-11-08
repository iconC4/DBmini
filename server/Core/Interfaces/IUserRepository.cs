using server.Application.DTOs;
using server.Core.Entities;

namespace server.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(string userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);
        Task<User> AddAsync(AddUserRequest addUserRequest);
    }
}