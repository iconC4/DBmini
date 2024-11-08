namespace server.Core.Entities
{
    public class Permission
    {
        public required string PermissionId { get; set; }
        public required string PermissionName { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }

        public required string UserId { get; set; }
    }
}
