using AsosiyProject.Application.SignUp.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;

[Authorize]
public class NotificationHub : Hub
{
    public async Task SendNotification(Guid userId, CreateNotificationDto notification)
    {
        await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
    }

    public async Task SendToAll(CreateNotificationDto notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }

    public async Task SendToGroup(string groupName, CreateNotificationDto notification)
    {
        await Clients.Group(groupName).SendAsync("ReceiveNotification", notification);
    }

    public async Task MarkAsRead(Guid notificationId)
    {
        await Clients.Caller.SendAsync("NotificationRead", notificationId);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}