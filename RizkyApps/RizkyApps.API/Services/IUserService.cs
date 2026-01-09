using RizkyApps.Shared.DTOs;

namespace RizkyApps.API.Services
{
    public interface IUserService
    {
        Task<DtoUserResponse?> GetUserByIdAsync(int id);
        Task<IEnumerable<DtoUserResponse>> GetAllUsersAsync();
        Task<DtoUserResponse?> GetUserAsync(LoginRequest UserDto);
        Task<DtoUserResponse?> CreateUserAsync(DtoUserCreate UserDto);
        //Task<DtoUserResponse?> UpdateUserAsync(int id, DtoUserUpdate UserDto);
        Task<DtoUserResponse?> UpdateUserAsync(DtoUserUpdate UserDto);
        Task<bool> DeleteUserAsync(int id);
    }
}