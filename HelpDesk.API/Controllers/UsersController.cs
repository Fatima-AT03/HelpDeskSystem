using HelpDesk.API.DTO;
using HelpDesk.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Controllers
{
     [ApiController]
     [Route("api/users")]
     [Authorize]
     public class UsersController : ControllerBase
     {
          private readonly IUserService _service;

          public UsersController(IUserService service)
          {
               _service = service;
          }

          [HttpGet]
          public async Task<IActionResult> GetUsers()
          {
               return Ok(await _service.GetUsers());
          }

          [HttpGet("{id}")]
          public async Task<IActionResult> GetUser(int id)
          {
               var user = await _service.GetUser(id);

               if (user == null)
                    return NotFound();

               return Ok(user);
          }

          [HttpPost]
          public async Task<IActionResult> CreateUser(CreateUserDto dto)
          {
               var user = await _service.CreateUser(dto);

               return Ok(user);
          }

          [HttpPut("{id}")]
          public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
          {
               var result = await _service.UpdateUser(id, dto);

               if (!result)
                    return NotFound();

               return NoContent();
          }

          [HttpDelete("{id}")]
          public async Task<IActionResult> DeleteUser(int id)
          {
               var result = await _service.DeleteUser(id);

               if (!result)
                    return NotFound();

               return NoContent();
          }

          [HttpPut("{id}/deactivate")]
          public async Task<IActionResult> DeactivateUser(int id)
          {
               var result = await _service.DeactivateUser(id);

               if (!result)
                    return NotFound();

               return NoContent();
          }
     }
}