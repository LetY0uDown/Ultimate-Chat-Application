using Host.Services;
using Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Host.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly ChatContext _context;
    private readonly JWTTokenGenerator _tokenGenerator;
    private readonly IPasswordEncoder _passwordEncoder;

    public AuthController (ChatContext database, JWTTokenGenerator tokenGenerator, IPasswordEncoder passwordEncoder)
    {
        _context = database;
        _tokenGenerator = tokenGenerator;
        _passwordEncoder = passwordEncoder;
    }

    [HttpGet("Authorize/{userID}/{pass}")]
    public async Task<ActionResult<object>> Authorize ([FromRoute] string userID, [FromRoute] string pass)
    {
        try {
            var u = await _context.Users.FindAsync(userID);

            if (u is null) {
                return NotFound("Пользователь не найден");
            }

            if (u.Password != pass) {
                return BadRequest("Неверный пароль");
            }

            var token = _tokenGenerator.GetToken(u.Username, pass);

            return new { u.ID, Token = token };
        } catch (Exception e) {
            return Problem(e.Message);
        }
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register ([FromBody] User user)
    {
        try {
            var isNameUnique = await _context.Users.AnyAsync(u => u.Username == user.Username);

            if (isNameUnique) {
                return BadRequest("Пользователь с таким именем уже существует. Попробуйте другое");
            }

            var pass = _passwordEncoder.Encode(user.Password);

            user.ID = Guid.NewGuid().ToString();
            user.Password = pass;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Authorize", "Auth", new { userID = user.ID, pass = user.Password });
        } catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpGet("Login")]
    public async Task<ActionResult> Login ([FromBody] User? user)
    {
        try {
            var userInBD = await _context.Users.FirstOrDefaultAsync(u => u.Username == user!.Username);

            if (userInBD is null) {
                return BadRequest("Такого пользователя не существует");
            }

            user.Password = _passwordEncoder.Encode(user.Password);

            return RedirectToAction("Authorize", "Auth", new { userID = userInBD.ID, pass = user.Password });
        } catch (Exception ex) {
            return Problem(ex.Message);
        }
    }
}