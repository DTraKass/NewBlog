using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBlog.Models;

namespace NewBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Получить всех пользователей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        // Получить пользователя по идентификатору
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // Регистрация пользователя
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(RegisterModel model)
        {
            var user = new  User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Присваиваем роль "Пользователь"
            await _userManager.AddToRoleAsync(user, "Пользователь");
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Редактирование пользователя
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.Email = model.Email;
            user.UserName = model.Email; // Обновление имени пользователя

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }

        // Удаление пользователя
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
