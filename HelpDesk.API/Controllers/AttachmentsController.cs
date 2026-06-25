using System.Threading.Tasks;
using HelpDesk.API.Data;
using HelpDesk.API.Models;
using HelpDesk.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : ControllerBase
    {
          private readonly IAttachmentService _attachmentService;

          public AttachmentsController(IAttachmentService attachmentService)
          {
               _attachmentService = attachmentService;
          }

          [HttpPost("{ticketId}")]
        public async Task<IActionResult> Upload(int ticketId, IFormFile file)
        {

               var attachment = await _attachmentService
                       .Upload(ticketId, file);

               if (attachment == null)
                    return BadRequest("No file selected.");

               return Ok(attachment);
          }

        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(int ticketId)
        {
            var attachments = await _attachmentService
                       .GetByTicket(ticketId);

            return Ok(attachments);
        }
    }
}