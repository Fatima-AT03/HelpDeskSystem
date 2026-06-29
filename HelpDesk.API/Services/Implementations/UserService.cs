using AutoMapper;
using HelpDesk.API.Data;
using HelpDesk.API.DTO;
using HelpDesk.API.Models;
using HelpDesk.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Services.Implementations
{
     public class UserService : IUserService
     {
          private readonly AppDbContext _context;
          private readonly IMapper _mapper;

          public UserService(AppDbContext context,
                             IMapper mapper)
          {
               _context = context;
               _mapper = mapper;
          }

          public async Task<List<UserDto>> GetUsers()
          {
               var users = await _context.Users
                   .OrderBy(u => u.FullName)
                   .ToListAsync();

               return _mapper.Map<List<UserDto>>(users);
          }

          public async Task<UserDto?> GetUser(int id)
          {
               var user = await _context.Users.FindAsync(id);

               if (user == null)
                    return null;

               return _mapper.Map<UserDto>(user);
          }

          public async Task<UserDto> CreateUser(CreateUserDto dto)
          {
               var user = new User
               {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Password = dto.Password,
                    RoleId = dto.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.Now
               };

               _context.Users.Add(user);

               await _context.SaveChangesAsync();

               return _mapper.Map<UserDto>(user);
          }

          public async Task<bool> UpdateUser(int id, UpdateUserDto dto)
          {
               var user = await _context.Users.FindAsync(id);

               if (user == null)
                    return false;

               user.FullName = dto.FullName;
               user.Email = dto.Email;
               user.RoleId = dto.RoleId;
               user.IsActive = dto.IsActive;

               await _context.SaveChangesAsync();

               return true;
          }

          public async Task<bool> DeleteUser(int id)
          {
               await using var transaction = await _context.Database.BeginTransactionAsync();

               try
               {
                    var user = await _context.Users.FindAsync(id);

                    if (user == null)
                         return false;

                    var notifications = await _context.Notifications
                        .Where(n => n.UserId == id)
                        .ToListAsync();

                    _context.Notifications.RemoveRange(notifications);

                    var comments = await _context.TicketComments
                        .Where(c => c.UserId == id)
                        .ToListAsync();

                    _context.TicketComments.RemoveRange(comments);

                    var tickets = await _context.Tickets
                        .Where(t => t.CreatedBy == id)
                        .ToListAsync();

                    foreach (var ticket in tickets)
                    {
                         var attachments = await _context.TicketAttachments
                             .Where(a => a.TicketId == ticket.Id)
                             .ToListAsync();

                         _context.TicketAttachments.RemoveRange(attachments);

                         var ticketComments = await _context.TicketComments
                             .Where(c => c.TicketId == ticket.Id)
                             .ToListAsync();

                         _context.TicketComments.RemoveRange(ticketComments);

                         var ticketNotifications = await _context.Notifications
                             .Where(n => n.TicketId == ticket.Id)
                             .ToListAsync();

                         _context.Notifications.RemoveRange(ticketNotifications);
                    }

                    _context.Tickets.RemoveRange(tickets);

                    var assignedTickets = await _context.Tickets
                        .Where(t => t.AssignedTo == id)
                        .ToListAsync();

                    foreach (var ticket in assignedTickets)
                    {
                         ticket.AssignedTo = null;
                    }

                    _context.Users.Remove(user);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
               }
               catch
               {
                    await transaction.RollbackAsync();
                    throw;
               }
          }

          public async Task<bool> DeactivateUser(int id)
          {
               var user = await _context.Users.FindAsync(id);

               if (user == null)
                    return false;

               user.IsActive = false;

               await _context.SaveChangesAsync();

               return true;
          }
     }
}