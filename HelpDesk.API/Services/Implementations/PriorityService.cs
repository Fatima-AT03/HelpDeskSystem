using HelpDesk.API.Data;
using HelpDesk.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Services.Implementations
{
     public class PriorityService : IPriorityService
     {
          private readonly AppDbContext _context;

          public PriorityService(AppDbContext context)
          {
               _context = context;
          }

          public async Task<IEnumerable<object>> GetPriorities()
          {
               return await _context.Priorities
                   .Select(p => new
                   {
                        p.Id,
                        p.Name
                   })
                   .ToListAsync();
          }
     }
}
