using server.Application.DTOs;
using server.Core.Entities;

namespace server.Core.Interfaces
{
    public interface IRoleRepository
    {
        Task DeleteAsync(Role role);
        Task<IEnumerable<Role>> GetRolesByUserId(string userId);
    }
}