using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Application.SignalR;

public class EnrollmentHub:Hub
{
    private readonly ILogger<ApplicantHub> _logger;

    public EnrollmentHub(ILogger<ApplicantHub> logger)
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

    public async Task AddToStudentGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Students");
        _logger.LogInformation($"Connection {Context.ConnectionId} added to group Students.");
    }

    public async Task RemoveFromStudentGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Students");
        _logger.LogInformation($"Connection {Context.ConnectionId} removed from group Students.");
    }

    public async Task NewLectureCapacity(CapacityDto Capacity) // Ensure method name is "NewUser"
    {
        await Clients.OthersInGroup("Students").SendAsync("NewLectureCapacity", Capacity);
        _logger.LogInformation("New New Lecture Capacity sent to group Students.");
    }

    public async Task NewSeminarCapacity(CapacityDto Capacity) // Ensure method name is "NewUser"
    {
        await Clients.OthersInGroup("Students").SendAsync("NewSeminarCapacity", Capacity);
        _logger.LogInformation("New New Seminar Capacity sent to group Students.");
    }
}
