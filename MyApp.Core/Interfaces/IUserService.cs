using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Core.Interfaces
{
    public record UserDto(int Id, string Name, string Email,string PasswordHash);
    public record CreateUserRequest(string Name, string Email, string Password);

    public interface IUserService
    {
        Task<UserDto?> GetUserAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> CreateUserAsync(CreateUserRequest request);

        Task<UserDto?> CheckUserAsync(int id,string password);

    }
}
