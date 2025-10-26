namespace EventManagementSystem.Application.Models.Participants;

public class ParticipantDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
