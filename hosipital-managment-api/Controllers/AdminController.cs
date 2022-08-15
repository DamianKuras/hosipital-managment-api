using hosipital_managment_api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hosipital_managment_api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApiUser> _userManager;
        public AdminController(AppDbContext context, UserManager<ApiUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> Get()
        {
            var users = _userManager.Users.ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = _context.Users.FindAsync(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(user);
        }

        [HttpDelete]
        [Route("user/delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id); 
            if (user==null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error when deleting user please try again latter");
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
