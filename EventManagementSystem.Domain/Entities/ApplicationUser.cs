using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public ICollection<Participant> Participations { get; set; } = [];
}
