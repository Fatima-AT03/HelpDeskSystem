namespace HelpDesk.API.DTOs
{
    public class DashboardDto
    {
        public int OpenTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int ResolvedTickets { get; set; }

        public int ClosedTickets { get; set; }

        public IEnumerable<object> RecentTickets { get; set; }
            = new List<object>();
    }
}