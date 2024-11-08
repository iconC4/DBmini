namespace server.Core.Models
{
    public class DataSource<T>
    {
        public required List<T> Items { get; set; }
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required int TotalCount { get; set; }
    }
}
