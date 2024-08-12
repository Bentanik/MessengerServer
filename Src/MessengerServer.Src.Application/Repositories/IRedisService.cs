using StackExchange.Redis;

namespace MessengerServer.Src.Application.Repositories;

public interface IRedisService
{
    Task SetStringAsync(string key, string value, TimeSpan expiry);
    Task<string> GetStringAsync(string key);
    Task<bool> DeleteKeyAsync(string key);
    Task HashSetFieldAsync(string id, string field, string value);
    Task<string> HashGetFieldAsync(string id, string field);
    Task<RedisValue[]> HashGetAllValuesAsync(string id);
    Task<RedisValue[]> HashGetAllFieldsAsync(string id);
    Task<bool> HashDeleteFieldAsync(string id, string field);
}
