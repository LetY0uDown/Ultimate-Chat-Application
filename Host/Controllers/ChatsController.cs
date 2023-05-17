using Host.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Host.Controllers;

[ApiController, Route("[controller]/"),
 Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatsController : ControllerBase
{
    private readonly ChatContext _context;
    private readonly IHubContext<ChatsHub> _hub;

    public ChatsController (ChatContext context, IHubContext<ChatsHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Chat>>> GetAllChats ()
    {
        try {
            return await _context.Chats.Include(c => c.Creator).ToListAsync();
        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpPost("Create"), Authorize]
    public async Task<ActionResult> CreateChat ([FromBody] Chat chat)
    {
        try {
            if (chat.Creator is not null) {
                _context.Users.Attach(chat.Creator);
            }

            chat.ID = Guid.NewGuid().ToString();

            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ChatCreated", chat);

            return Ok();

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{chatID}/Delete"), Authorize]
    public async Task<ActionResult> DeleteChat ([FromRoute] string chatID)
    {
        try {
            var chat = await _context.Chats.SingleOrDefaultAsync(c => c.ID == chatID);

            if (chat is null) {
                return NotFound("Ошибка: чат не найден");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ChatDeleted", chat);

            return NoContent();

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpPut("Edit"), Authorize]
    public async Task<ActionResult> EditChat ([FromBody] Chat chat)
    {
        try {
            _context.Entry(chat).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpPost("{chatID}/Join/{userID}"), Authorize]
    public async Task<ActionResult> JoinChat ([FromRoute] string chatID,
                                              [FromRoute] string userID)
    {
        try {
            var chatMember = await _context.ChatMembers.FirstOrDefaultAsync(e => e.ChatId == chatID && e.MemberId == userID);

            if (chatMember is not null) {
                return NoContent();
            }

            chatMember = new ChatMember {
                ChatId = chatID,
                MemberId = userID
            };

            await _context.ChatMembers.AddAsync(chatMember);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userID);

            await _hub.Clients.Group($"Chat - {chatID}").SendAsync("UserJoined", user);

            return Ok();

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpPost("{chatID}/Leave/{userID}"), Authorize]
    public async Task<ActionResult> LeaveChat ([FromRoute] string chatID,
                                               [FromRoute] string userID)
    {
        try {
            var chatMember = await _context.ChatMembers.FirstOrDefaultAsync(e => e.ChatId == chatID && e.MemberId == userID);

            _context.ChatMembers.Remove(chatMember!);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userID);

            await _hub.Clients.Group($"Chat - {chatID}").SendAsync("UserLeft", user);

            return Ok();

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{chatID}/Messages"), Authorize]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessagesFromChat ([FromRoute] string chatID)
    {
        try {
            var doesChatExist = await _context.Chats.AnyAsync(c => c.ID == chatID);

            if (!doesChatExist) {
                return NotFound("Чат не найден");
            }

            return await _context.Messages.Where(m => m.ChatId == chatID)
                                          .OrderBy(m => m.SendedDate)
                                          .ToListAsync();
        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{chatID}/Members"), Authorize]
    public async Task<ActionResult<IEnumerable<Message>>> GetMembersFromChat ([FromRoute] string chatID)
    {
        try {
            var doesChatExist = await _context.Chats.AnyAsync(c => c.ID == chatID);

            if (!doesChatExist) {
                return NotFound("Чат не найден");
            }

            var members = await _context.Users.Include(u => u.Chats)
                                              .Where(u => u.Chats
                                              .Any(c => c.ID == chatID))
                                              .ToListAsync();

            return Ok(members);

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpPatch("{chatID}/Send/{userID}"), Authorize]
    public async Task<ActionResult> SendMessage ([FromRoute] string chatID,
                                                 [FromBody] string text,
                                                 [FromRoute] string userID)
    {
        try {
            var sender = await _context.Users.FindAsync(userID);

            var message = new Message {
                ID = Guid.NewGuid().ToString(),
                Text = text,
                SendedDate = DateTime.UtcNow,
                Sender = sender,
                SenderId = userID,
                ChatId = chatID
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            await _hub.Clients.Group($"Chat - {chatID}").SendAsync("MessageSended", message);

            return Ok();

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{chatID}/Delete/{messageID}"), Authorize]
    public async Task<ActionResult> DeleteMessage ([FromRoute] string chatID,
                                                   [FromRoute] string messageID)
    {
        try {
            var msg = await _context.Messages.FindAsync(messageID);

            _context.Messages.Remove(msg);
            await _context.SaveChangesAsync();

            await _hub.Clients.Group($"Chat - {chatID}").SendAsync("MessageDeleted", msg);

            return Ok();

        }
        catch (Exception ex) {
            return Problem(ex.Message);
        }
    }
}