using server.Core.Entities;
using server.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using server.Infrastructure.Data;

namespace server.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        // เพิ่มข้อมูล Role ใหม่
        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        // ดึงข้อมูล Roles ทั้งหมด
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        // ดึง Role ตาม UserId
        public async Task<IEnumerable<Role>> GetRolesByUserId(string userId)
        {
            return await _context.Roles
                .Where(r => r.Users.Any(u => u.UserId == userId))
                .ToListAsync();
        }

        // อัปเดต Role
        public async Task<Role> UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        // ลบ Role
        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}
