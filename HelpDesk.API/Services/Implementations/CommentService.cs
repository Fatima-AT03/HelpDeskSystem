using AutoMapper;
using HelpDesk.API.Data;
using HelpDesk.API.DTO;
using HelpDesk.API.Models;
using HelpDesk.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Services.Implementations
{
     public class CommentService : ICommentService
     {
          private readonly AppDbContext _context;
          private readonly IMapper _mapper;

          public CommentService(AppDbContext context, IMapper mapper)
          {
               _context = context;
               _mapper = mapper;
          }

          public async Task<IEnumerable<TicketCommentDto>> GetComments(int ticketId)
          {
               var comments = await _context.TicketComments
                   .Include(c => c.User)
                   .Where(c => c.TicketId == ticketId)
                   .OrderBy(c => c.CreatedAt)
                   .ToListAsync();

               return _mapper.Map<List<TicketCommentDto>>(comments);
          }

          public async Task<bool> AddComment(
              CreateCommentDto dto,
              int userId)
          {
               var comment = _mapper.Map<TicketComment>(dto);

               comment.UserId = userId;
               comment.CreatedAt = DateTime.UtcNow;

               _context.TicketComments.Add(comment);

               var ticket = await _context.Tickets
                   .FirstOrDefaultAsync(t => t.Id == dto.TicketId);

               if (ticket != null)
               {
                    if (ticket.CreatedBy != userId)
                    {
                         _context.Notifications.Add(new Notification
                         {
                              UserId = ticket.CreatedBy,
                              TicketId = ticket.Id,
                              Message =
                                 $"A new comment was added to ticket {ticket.ReferenceNumber}.",
                              IsRead = false,
                              CreatedAt = DateTime.UtcNow
                         });
                    }

                    if (ticket.AssignedTo.HasValue &&
                        ticket.AssignedTo.Value != userId)
                    {
                         _context.Notifications.Add(new Notification
                         {
                              UserId = ticket.AssignedTo.Value,
                              TicketId = ticket.Id,
                              Message =
                                 $"A new comment was added to ticket {ticket.ReferenceNumber}.",
                              IsRead = false,
                              CreatedAt = DateTime.UtcNow
                         });
                    }
               }

               await _context.SaveChangesAsync();

               return true;
          }

          public async Task<TicketComment?> UpdateComment(
              int id,
              UpdateCommentDto dto,
              int userId)
          {
               var comment = await _context.TicketComments
                   .FirstOrDefaultAsync(c => c.Id == id);

               if (comment == null)
                    return null;

               if (comment.UserId != userId)
                    return null;

               _mapper.Map(dto, comment);

               await _context.SaveChangesAsync();

               return comment;
          }

          public async Task<bool> DeleteComment(int id)
          {
               var comment = await _context.TicketComments
                   .FirstOrDefaultAsync(c => c.Id == id);

               if (comment == null)
                    return false;

               _context.TicketComments.Remove(comment);

               await _context.SaveChangesAsync();

               return true;
          }
     }
}