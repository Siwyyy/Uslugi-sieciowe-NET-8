using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody] LoginModel newUser)
        {
            newUser.Role = "Admin";
            UserConstants.Users.Add(newUser);
            return Ok(new { message = $"Użytkownik {newUser.Username} został pomyślnie dodany do systemu!" });
        }
    }
}
