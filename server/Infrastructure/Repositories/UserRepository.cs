using server.Core.Entities;
using server.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using server.Infrastructure.Data;
using server.Application.DTOs;

namespace server.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Permissions).ToListAsync();
        }

        public async Task<User?> GetUserById(string id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User> AddAsync(AddUserRequest addUserRequest)
        {
            throw new NotImplementedException();
        }
    }
}
