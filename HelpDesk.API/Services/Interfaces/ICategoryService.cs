namespace HelpDesk.API.Services.Interfaces
{
     public interface ICategoryService
     {
          Task<IEnumerable<object>> GetCategories();
     }
}