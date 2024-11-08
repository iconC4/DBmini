using System.Text.Json.Serialization;

namespace server.Core.Entities
{
    public class User
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string RoleId { get; set; }
        public required Role? Role { get; set; }
        public required List<Permission> Permissions { get; set; }
    }
}
