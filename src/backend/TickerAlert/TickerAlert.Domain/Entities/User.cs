using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

public class User : Entity
{
    public string Username { get; private set; }
    public string HashedPassword { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public bool IsActive { get; private set; }

    private User(Guid id, string username, string hashedPassword) : base(id)
    {
        Username = username;
        HashedPassword = hashedPassword;
        CreatedOn = DateTime.UtcNow;
        IsActive = true;
    }
    
    public static User CreateUser(Guid Id, string username, string hashedPassword)
    {
        return new User(Id, username, hashedPassword);
    }
}