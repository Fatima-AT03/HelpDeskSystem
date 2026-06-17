using HelpDesk.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ReportsController : ControllerBase
{
     private readonly AppDbContext _context;

     public ReportsController(AppDbContext context)
     {
          _context = context;
     }

     [HttpGet]
     public async Task<IActionResult> GetReportData()
     {
          var totalTickets = _context.Tickets.Count();

          var openTickets = _context.Tickets
              .Count(t => t.Status.Name == "Open");

          var inProgressTickets = _context.Tickets
              .Count(t => t.Status.Name == "In Progress");

          var pendingTickets = _context.Tickets
              .Count(t => t.Status.Name == "Pending");

          var resolvedTickets = _context.Tickets
              .Count(t => t.Status.Name == "Resolved");

          var closedTickets = _context.Tickets
              .Count(t => t.Status.Name == "Closed");

          var supportAgentsOnline = _context.Users
            .Count(u => u.RoleId == 3);

          var lowTickets = _context.Tickets
             .Count(t => t.Priority.Name == "Low");

          var mediumTickets = _context.Tickets
              .Count(t => t.Priority.Name == "Medium");

          var highTickets = _context.Tickets
              .Count(t => t.Priority.Name == "High");

          var criticalTickets = _context.Tickets
              .Count(t => t.Priority.Name == "Critical");

          var assignedTickets = _context.Tickets
               .Count(t => t.AssignedTo != null);

          var unassignedTickets = _context.Tickets
               .Count(t => t.AssignedTo == null);

          return Ok(new
          {
               totalTickets,
               openTickets,
               inProgressTickets,
               pendingTickets,
               resolvedTickets,
               closedTickets,
               supportAgentsOnline,
               lowTickets,
               mediumTickets,
               highTickets,
               criticalTickets,
               assignedTickets,
               unassignedTickets
          });
     }
}