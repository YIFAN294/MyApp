using Microsoft.Extensions.Logging;
using MyApp.Core.Interfaces;
using MyApp.Models;
using BCrypt;

namespace MyApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        _logger.LogInformation("获取用户 ID: {UserId}", id);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        return MapToDto(user);
    }

    public async Task<UserDto?> CheckUserAsync(int id,string password)
    {
        _logger.LogInformation("获取用户 ID: {UserId}", id);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        string storedHash = user.PasswordHash;

        bool isValid = BCrypt.Net.BCrypt.Verify(password, storedHash);

        if (!isValid)
        {
            _logger.LogWarning("密码不对: {password}", password);
            return null;
        } 

        return MapToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("获取所有用户");

        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<bool> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogInformation("创建用户: {Email}", request.Email);

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("邮箱已存在: {Email}", request.Email);
            return false;
        }


        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);
        _logger.LogInformation("用户创建成功: {UserId}", user.Id);

        return true;
    }

    private static UserDto MapToDto(User user) =>
        new(user.Id, user.Name, user.Email,user.PasswordHash);

}
