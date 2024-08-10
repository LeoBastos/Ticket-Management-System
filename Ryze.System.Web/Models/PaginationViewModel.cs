namespace Ryze.System.Web.Models
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string? Area { get; set; }

        public int PageNumber { get; set; }
        public int TotalItems { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
