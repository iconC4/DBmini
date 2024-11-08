namespace server.Application.DTOs
{
    public class AddUserRequest
    {
        public string? UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
        public required List<PermissionRequest> Permissions { get; set; }
    }
}