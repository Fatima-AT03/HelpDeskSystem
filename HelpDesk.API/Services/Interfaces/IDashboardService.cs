using HelpDesk.API.DTOs;

namespace HelpDesk.API.Services.Interfaces
{
     public interface IDashboardService
     {
          Task<DashboardDto> GetDashboard();
     }
}