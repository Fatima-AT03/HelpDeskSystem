using AutoMapper;
using HelpDesk.API.Data;
using HelpDesk.API.DTOs;
using HelpDesk.API.Models;
using HelpDesk.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Services.Implementations
{
     public class TicketService : ITicketService
     {
          private readonly AppDbContext _context;
          private readonly IMapper _mapper;

          public TicketService(AppDbContext context, IMapper mapper)
          {
               _context = context;
               _mapper = mapper;
          }

          public async Task<IEnumerable<TicketDto>> GetTickets(
              string role,
              int userId)
          {
               var query = _context.Tickets
                   .Include(t => t.Category)
                   .Include(t => t.Priority)
                   .Include(t => t.Status)
                   .Include(t => t.Creator)
                   .Include(t => t.Assignee)
                   .AsQueryable();

               if (role == "Employee")
                    query = query.Where(t => t.CreatedBy == userId);

               else if (role == "Support Agent")
                    query = query.Where(t => t.AssignedTo == userId);

               var tickets = await query.ToListAsync();
               return _mapper.Map<IEnumerable<TicketDto>>(tickets);
          }

          public async Task<TicketDto?> GetTicket(int id)
          {
               var ticket =  await _context.Tickets
                   .Include(t => t.Category)
                   .Include(t => t.Priority)
                   .Include(t => t.Status)
                   .Include(t => t.Creator)
                   .Include(t => t.Assignee)
                   .FirstOrDefaultAsync(t => t.Id == id);

               return _mapper.Map<TicketDto>(ticket);
          }

          public async Task<Ticket> CreateTicket(CreateTicketDto dto, int userId)
          {
               var ticket = _mapper.Map<Ticket>(dto);

               ticket.CreatedBy = userId;
               ticket.CreatedAt = DateTime.UtcNow;
               ticket.UpdatedAt = DateTime.UtcNow;
               ticket.StatusId = 1;
               ticket.ReferenceNumber = $"TKT-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

               _context.Tickets.Add(ticket);
               await _context.SaveChangesAsync();

               _context.Notifications.Add(new Notification
               {
                    UserId = userId,
                    TicketId = ticket.Id,
                    Message = $"Your ticket ({ticket.ReferenceNumber}) has been created successfully.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
               });

               var adminsAndManagers = await _context.Users
                   .Where(u => u.RoleId == 1 || u.RoleId == 4)
                   .ToListAsync();

               foreach (var user in adminsAndManagers)
               {
                    if (user.Id == userId)
                         continue;

                    _context.Notifications.Add(new Notification
                    {
                         UserId = user.Id,
                         TicketId = ticket.Id,
                         Message = $"A new ticket ({ticket.ReferenceNumber}) has been created.",
                         IsRead = false,
                         CreatedAt = DateTime.UtcNow
                    });
               }

               await _context.SaveChangesAsync();

               return ticket;
          }


          public async Task<Ticket?> UpdateTicket(int id, Ticket updated, string role, int userId)
          {
               var ticket = await _context.Tickets.FindAsync(id);

               if (ticket == null)
                    return null;

               if (role == "Employee")
               {
                    if (ticket.StatusId != 1)
                         return null;

                    if (ticket.CreatedBy != userId)
                         return null;
               }

               ticket.Title = updated.Title;
               ticket.Description = updated.Description;
               ticket.Priority = updated.Priority;

               await _context.SaveChangesAsync();

               return ticket;
          }

          public async Task<IEnumerable<AssignableUserDto>> GetAssignableUsers()
          {
               return await _context.Users
                   .Where(u =>
                       u.RoleId == 3 ||
                       u.RoleId == 4)
                   .Select(u => new AssignableUserDto
                   {
                        Id = u.Id,
                        FullName = u.FullName,
                        RoleId = u.RoleId,
                        AssignedTicketsCount =
                           _context.Tickets.Count(
                               t => t.AssignedTo == u.Id)
                   })
                   .OrderBy(u => u.AssignedTicketsCount)
                   .ThenBy(u => u.FullName)
                   .ToListAsync();
          }

          public async Task<(bool Success, string Message)> AssignTicket(int id, AssignTicketDto dto)
          {
               var ticket = await _context.Tickets
                   .FirstOrDefaultAsync(t => t.Id == id);

               if (ticket == null)
                    return (false, "Ticket not found");

               var user = await _context.Users
                   .FirstOrDefaultAsync(u => u.Id == dto.UserId);

               if (user == null)
                    return (false, "User not found");

               if (user.RoleId != 3 &&
                   user.RoleId != 4)
               {
                    return (false,
                        "Can only assign to managers or support agents");
               }

               ticket.AssignedTo = dto.UserId;
               ticket.UpdatedAt = DateTime.UtcNow;

               _context.Notifications.Add(new Notification
               {
                    UserId = dto.UserId,
                    TicketId = ticket.Id,
                    Message =
                       $"A ticket ({ticket.ReferenceNumber}) has been assigned to you.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
               });

               await _context.SaveChangesAsync();

               return (true, "Ticket assigned");
          }

          public async Task<(bool Success, string Message)>UpdateStatus(int id, string status, string role)
          {
               var ticket =
                   await _context.Tickets.FindAsync(id);

               if (ticket == null)
                    return (false, "Ticket not found");

               List<string> allowedStatuses = new();

               if (role == "Admin" || role == "Manager")
               {
                    allowedStatuses = new()
        {
            "in progress",
            "pending",
            "resolved",
            "closed"
        };
               }
               else if (role == "Support Agent")
               {
                    allowedStatuses = new()
        {
            "in progress",
            "pending",
            "resolved"
        };
               }
               else
               {
                    return (false, "Forbidden");
               }

               if (!allowedStatuses.Contains(status.ToLower()))
               {
                    return (false, "Status not allowed");
               }

               var statusEntity =
                   await _context.Statuses
                       .FirstOrDefaultAsync(s =>
                           s.Name.ToLower() ==
                           status.ToLower());

               if (statusEntity == null)
                    return (false, "Invalid status");

               ticket.StatusId = statusEntity.Id;
               ticket.UpdatedAt = DateTime.UtcNow;

               _context.Notifications.Add(new Notification
               {
                    UserId = ticket.CreatedBy,
                    TicketId = ticket.Id,
                    Message =
                       $"Your ticket ({ticket.ReferenceNumber}) status changed to {statusEntity.Name}.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
               });

               await _context.SaveChangesAsync();

               return (true, "Status updated successfully");
          }

          public async Task<(bool Success, string Message)> DeleteTicket(int id, string role, int userId)
          {
               var ticket = await _context.Tickets.FindAsync(id);

               if (ticket == null)
                    return (false, "Ticket not found.");

               if (role == "Employee")
               {
                    if (ticket.StatusId != 1) 
                         return (false, "You can only delete Open tickets.");

                    if (ticket.CreatedBy != userId)
                         return (false, "You can only delete your own tickets.");
               }

               var hasComments = await _context.TicketComments
                                   .AnyAsync(c => c.TicketId == id);

               if (hasComments)
               {
                    return (false, "The ticket has comments and can't be deleted.");
               }

               _context.Tickets.Remove(ticket);
               await _context.SaveChangesAsync();

               return (true, "Ticket deleted successfully.");
          }
     }
}