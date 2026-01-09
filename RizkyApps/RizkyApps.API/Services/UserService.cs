using Microsoft.EntityFrameworkCore;
using RizkyApps.API.Data;
using RizkyApps.Shared.DTOs;
using RizkyApps.API.Models;

namespace RizkyApps.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DtoUserResponse>> GetAllUsersAsync()
        {
            var Users = await _context.MsUsers
                .OrderBy(p => p.Id)
                .ToListAsync();

            return Users.Select(p => MapToDto(p));
        }

        public async Task<DtoUserResponse?> GetUserByIdAsync(int id)
        {
            var User = await _context.MsUsers.FindAsync(id);
            return User != null ? MapToDto(User) : null;
        }

        public async Task<DtoUserResponse?> CreateUserAsync(DtoUserCreate UserDto)
        {
            var currentUser = await _context.MsUsers.Where(x => x.Username.Equals(UserDto.Username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync();

            if (currentUser is null)
            {
                var User = new MsUser
                {
                    Username = UserDto.Username,
                    Password = UserDto.Password,
                    Name = UserDto.Name,
                    Email = UserDto.Email,
                    Phone = UserDto.Phone,
                    Birthday = UserDto.Birthday,
                    CreatedBy = UserDto.Username,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = UserDto.Username,
                    ModifiedDate = DateTime.UtcNow
                };

                _context.MsUsers.Add(User);
                await _context.SaveChangesAsync();

                return MapToDto(User);
            }

            return null;

        }

        //public async Task<DtoUserResponse?> UpdateUserAsync(int id, DtoUserUpdate UserDto)
        public async Task<DtoUserResponse?> UpdateUserAsync(DtoUserUpdate UserDto)
        {
            var User = await _context.MsUsers.FindAsync(UserDto.Id);
            if (User == null) return null;

            // Update only provided fields using C# 10's null-coalescing assignment
            User.Username = UserDto.Username;
            User.Password = UserDto.Password;
            User.Name = UserDto.Name;
            User.Email = UserDto.Email;
            User.Phone = UserDto.Phone;
            User.Birthday = UserDto.Birthday;
            User.ModifiedBy = UserDto.Username;
            User.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(User);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var User = await _context.MsUsers.FindAsync(id);
            if (User == null) return false;

            _context.MsUsers.Remove(User);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DtoUserResponse?> GetUserAsync(LoginRequest UserDto)
        {
            var currentUser = await _context.MsUsers
                .Where(x => x.Username == UserDto.Username && x.Password == UserDto.Password)
                .FirstOrDefaultAsync();

            return MapToDto(currentUser);
        }

        private static DtoUserResponse? MapToDto(MsUser? User)
        {
            if (User is not null)
            {
                return new DtoUserResponse(
                    User.Id,
                    User.Name,
                    User.Email,
                    User.Phone,
                    User.Birthday
                );
            }
            return null;
        }

    }
}