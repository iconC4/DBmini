namespace server.Core.Models
{
    public class Response<T>
    {
        public required Status Status { get; set; }
        public required T Data { get; set; }
    }

    public class Status
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
    }
}
