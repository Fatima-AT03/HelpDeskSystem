namespace HelpDesk.API.Services.Interfaces
{
     public interface IPriorityService
     {
          Task<IEnumerable<object>> GetPriorities();
     }
}
