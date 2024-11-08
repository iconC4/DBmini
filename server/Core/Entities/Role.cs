using System.Text.Json.Serialization;

namespace server.Core.Entities
{
    public class Role
    {
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
        [JsonIgnore] // ป้องกันการวนซ้ำ
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
