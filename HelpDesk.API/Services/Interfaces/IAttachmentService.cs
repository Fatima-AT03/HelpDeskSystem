using HelpDesk.API.Models;

namespace HelpDesk.API.Services.Interfaces
{
     public interface IAttachmentService
     {
          Task<TicketAttachment?> Upload(int ticketId, IFormFile file);

          Task<IEnumerable<TicketAttachment>>GetByTicket(int ticketId);
     }
}