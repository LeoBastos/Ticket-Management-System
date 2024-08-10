namespace Ryze.System.Web.Models.Tickets
{
    public class TicketListViewModel
    {
        public List<TicketViewModel> Tickets { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
        public int TotalItems { get; set; }
    }
}
