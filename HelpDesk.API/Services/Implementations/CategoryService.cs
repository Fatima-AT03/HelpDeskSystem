using HelpDesk.API.Data;
using HelpDesk.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Services.Implementations
{
     public class CategoryService : ICategoryService
     {
          private readonly AppDbContext _context;

          public CategoryService(AppDbContext context)
          {
               _context = context;
          }

          public async Task<IEnumerable<object>> GetCategories()
          {
               return await _context.Categories
                   .Select(c => new
                   {
                        c.Id,
                        c.Name
                   })
                   .ToListAsync();
          }
     }
}