using server.Core.Entities;

namespace server.Core.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetPermissionsByUserId(string userId);
        Task<Permission> AddAsync(Permission permission);
        Task<Permission> UpdateAsync(Permission permission);
        Task DeleteAsync(Permission permission);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetPermissionById(string id);
    }
}
