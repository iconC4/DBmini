namespace server.Application.DTOs
{
    public class GetUsersRequest
    {
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Search { get; set; }
    }
}
