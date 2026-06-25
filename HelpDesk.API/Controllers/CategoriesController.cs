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
     public class CategoriesController : ControllerBase
     {
          private readonly ICategoryService _categoryService;

          public CategoriesController(
              ICategoryService categoryService)
          {
               _categoryService = categoryService;
          }

          [HttpGet]
          public async Task<IActionResult> GetCategories()
          {
               var categories =
                   await _categoryService.GetCategories();

               return Ok(categories);
          }
     }
}