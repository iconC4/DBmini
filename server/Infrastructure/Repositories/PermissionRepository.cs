using server.Core.Entities;
using server.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using server.Infrastructure.Data;

namespace server.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByUserId(string userId)
        {
            return await _context.Permissions
                .Where(p => p.UserId == userId) // ตรวจสอบให้แน่ใจว่ามี UserId ใน Permission
                .ToListAsync();
        }

        public async Task<Permission> AddAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetPermissionById(string id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task DeleteAsync(Permission permission)
        {
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<Permission> UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
            return permission;
        }
    }
}
