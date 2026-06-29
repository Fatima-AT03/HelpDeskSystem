using HelpDesk.API.DTO;

namespace HelpDesk.API.Services.Interfaces
{
     public interface IUserService
     {
          Task<List<UserDto>> GetUsers();

          Task<UserDto?> GetUser(int id);

          Task<UserDto> CreateUser(CreateUserDto dto);

          Task<bool> UpdateUser(int id, UpdateUserDto dto);

          Task<bool> DeleteUser(int id);

          Task<bool> DeactivateUser(int id);
     }
}