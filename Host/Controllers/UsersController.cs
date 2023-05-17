using Host.Services;
using Host.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Host.Controllers;

[ApiController, Route("[controller]/")]
public class UsersController : ControllerBase
{
    private readonly ChatContext _context;
    private readonly IPasswordEncoder _passwordEncoder;

    public UsersController (ChatContext context, IPasswordEncoder passwordEncoder)
    {
        _context = context;
        _passwordEncoder = passwordEncoder;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers ()
    {
        try {
            return await _context.Users.ToListAsync();
        } catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpPut("{userID}/Update"), Authorize]
    public async Task<ActionResult> UpdateUser ([FromBody] User user)
    {
        try {
            user.Password = _passwordEncoder.Encode(user.Password);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{userID}/Delete"), Authorize]
    public async Task<ActionResult> DeleteUser ([FromRoute] string userID)
    {
        try {
            var user = await _context.Users.FindAsync(userID);
            _context.Users.Remove(user!);

            await _context.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex) {
            return Problem(ex.Message);
        }
    }
}