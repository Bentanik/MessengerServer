using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MessengerServer.Src.Infrastructure.Services;

public class RedisService : IRedisService, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly RedisSetting _redisSetting;
    private readonly IDatabase _db;
    private bool _disposed = false;

    public RedisService(IOptions<RedisSetting> redisConfig)
    {
        _redisSetting = redisConfig.Value;
        _redis = ConnectionMultiplexer.Connect(_redisSetting.DefaultConnection);
        _db = _redis.GetDatabase();
    }

    public async Task SetStringAsync(string key, string value, TimeSpan expiry)
    {
        await _db.StringSetAsync(key, value, expiry);
    }

    public async Task<string> GetStringAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task<bool> DeleteKeyAsync(string key)
    {
        return await _db.KeyDeleteAsync(key);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _redis?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~RedisService()
    {
        Dispose(false);
    }

}