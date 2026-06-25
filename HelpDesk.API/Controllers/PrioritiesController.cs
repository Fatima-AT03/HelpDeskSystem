using HelpDesk.API.Data;
using HelpDesk.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Controllers
{
     [ApiController]
     [Route("api/[controller]")]
     [Authorize]
     public class PrioritiesController : ControllerBase
     {
          private readonly IPriorityService _priorityService;

          public PrioritiesController(
              IPriorityService priorityService)
          {
               _priorityService = priorityService;
          }

          [HttpGet]
          public async Task<IActionResult> GetPriorities()
          {
               var priorities = await _priorityService.GetPriorities();

               return Ok(priorities);
          }
     }
}