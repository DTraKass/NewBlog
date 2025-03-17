using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewBlog.Models;
using System.Security.Claims;

namespace NewBlog.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AuthentificationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }.Concat(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var claimsIdentity = new ClaimsIdentity(claims, "login");
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }

            return Unauthorized();
        }
    }
}
