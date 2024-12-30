using StackExchange.Redis;

public static class RedisExtensions
{
    public static void FlushDb(this IConnectionMultiplexer redisConnection)
    {
        var db = redisConnection.GetDatabase();
        db.Execute("FLUSHDB");
    }
}