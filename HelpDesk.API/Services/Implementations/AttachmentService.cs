using HelpDesk.API.Data;
using HelpDesk.API.Models;
using HelpDesk.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Services.Implementations
{
     public class AttachmentService : IAttachmentService
     {
          private readonly AppDbContext _context;
          private readonly IWebHostEnvironment _environment;

          public AttachmentService(
              AppDbContext context,
              IWebHostEnvironment environment)
          {
               _context = context;
               _environment = environment;
          }

          public async Task<TicketAttachment?> Upload(int ticketId, IFormFile file)
          {
               if (file == null || file.Length == 0)
                    return null;

               var uploadsFolder =
                   Path.Combine(_environment.ContentRootPath, "Uploads");

               if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

               var uniqueFileName =
                   $"{Guid.NewGuid()}_{file.FileName}";

               var filePath =
                   Path.Combine(uploadsFolder, uniqueFileName);

               using var stream =
                   new FileStream(filePath, FileMode.Create);

               await file.CopyToAsync(stream);

               var attachment = new TicketAttachment
               {
                    TicketId = ticketId,
                    FileName = file.FileName,
                    FilePath = $"/attachments/{uniqueFileName}",
                    UploadedAt = DateTime.UtcNow
               };

               _context.TicketAttachments.Add(attachment);

               await _context.SaveChangesAsync();

               return attachment;
          }

          public async Task<IEnumerable<TicketAttachment>>GetByTicket(int ticketId)
          {
               return await _context.TicketAttachments
                   .Where(a => a.TicketId == ticketId)
                   .ToListAsync();
          }
     }
}