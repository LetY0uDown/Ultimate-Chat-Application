using Microsoft.AspNetCore.SignalR;

namespace Host.Services;

public class ChatsHub : Hub
{
    public async Task JoinGroup (string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat {groupName}");
    }

    public async Task LeaveGroup (string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat {groupName}");
    }
}