using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Application.SignalR;
public class ApplicantHub : Hub
{
    private readonly ILogger<ApplicantHub> _logger;

    public ApplicantHub(ILogger<ApplicantHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("A client connected with connection ID: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("A client disconnected with connection ID: {ConnectionId}", Context.ConnectionId);
        if (exception != null)
        {
            _logger.LogError(exception, "An error occurred during disconnection.");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task AddToAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        _logger.LogInformation($"Connection {Context.ConnectionId} added to group Admins.");
    }

    public async Task RemoveFromAdminGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
        _logger.LogInformation($"Connection {Context.ConnectionId} removed from group Admins.");
    }

    public async Task NewUser(ApplicantDto user) // Ensure method name is "NewUser"
    {
        if (user == null)
        {
            _logger.LogError("Attempted to send a null user object.");
            throw new ArgumentNullException(nameof(user));
        }

        await Clients.Group("Admins").SendAsync("NewUser", user);
        _logger.LogInformation("New user sent to group Admins.");
    }
}
