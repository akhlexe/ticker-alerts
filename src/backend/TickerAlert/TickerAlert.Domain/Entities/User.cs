using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

public class User : Entity
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}