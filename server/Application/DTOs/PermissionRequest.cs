namespace server.Application.DTOs
{
    public class PermissionRequest
    {
        public required string PermissionId { get; set; }
        public required string PermissionName { get; set; }
        public required bool IsReadable { get; set; }
        public required bool IsWritable { get; set; }
        public required bool IsDeletable { get; set; }
    }
}
