using HelpDesk.API.Data;
using HelpDesk.API.DTOs;
using HelpDesk.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    ReferenceNumber = t.ReferenceNumber,
                    Title = t.Title,
                    Description = t.Description,
                    Category = t.Category!.Name,
                    Priority = t.Priority!.Name,
                    Status = t.Status!.Name,
                    CreatedBy = t.Creator!.FullName,
                    AssignedTo = t.Assignee != null
                        ? t.Assignee.FullName
                        : null,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(
            Ticket ticket)
        {
            ticket.ReferenceNumber =
                $"TKT-{DateTime.UtcNow:yyyyMMddHHmmss}";

            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(
            int id,
            Ticket updated)
        {
            var ticket =
                await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return NotFound();

            ticket.Title = updated.Title;
            ticket.Description = updated.Description;
            ticket.CategoryId = updated.CategoryId;
            ticket.PriorityId = updated.PriorityId;
            ticket.StatusId = updated.StatusId;
            ticket.AssignedTo = updated.AssignedTo;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}