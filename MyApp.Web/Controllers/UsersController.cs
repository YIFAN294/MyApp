using Microsoft.AspNetCore.Mvc;
using MyApp.Core.Interfaces;

namespace MyApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            _logger.LogInformation("GET /api/users");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}/{password}")]
        public async Task<ActionResult<UserDto>> CheckPassword(int id,string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest("密码不能为空");

            var user = await _userService.CheckUserAsync(id, password);
            if (user == null)
            {
                return Conflict(new { message = "密码不对" }); ;
            } 
            return Ok(user);
        }

        [HttpGet("verify/{id}")]
        public async Task<ActionResult<UserDto>> CheckPassword_ex(int id,[FromQuery] string password)  // 用 Query 参数更安全一点
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest("密码不能为空");

            var user = await _userService.CheckUserAsync(id, password);
            if (user == null)
                return Unauthorized("用户不存在或密码错误");  

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserRequest request)
        {
            var success = await _userService.CreateUserAsync(request);
            if (!success) return Conflict(new { message = "邮箱已存在" });
            return CreatedAtAction(nameof(Get), new { id = 0 }, request);
        }
    }
}
