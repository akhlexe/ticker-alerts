using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Entities;

public class Watchlist : Entity
{
    public const string DEFAULT_WATCHLIST_NAME = "main";

    public Guid UserId { get; private set; }
    public string Name { get; private set; }

    public static Watchlist Create(Guid id, Guid userId, string name = DEFAULT_WATCHLIST_NAME) => new(id, userId, name);

    private Watchlist(Guid id, Guid userId, string name)
            : base(id)
    {
        UserId = userId;
        Name = name;
    }
}
