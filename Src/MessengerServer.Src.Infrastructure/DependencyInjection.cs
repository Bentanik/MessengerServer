using MessengerServer.Src.Application;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Infrastructure.Repositories;
using MessengerServer.Src.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerServer.Src.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<INotificationAddFriendRepository, NotificationAddFriendRepository>();
        services.AddScoped<IMessageRepository,MessageRepository>();
        services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
        services.AddScoped<IHubNotificationService, HubNotificationService>();
        services.AddScoped<IHubMessageService, HubMessageService>();
        services.AddScoped<IPasswordHashService, PasswordHash>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailServices, EmailService>();
        services.AddScoped<IRedisService, RedisService>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IMediaService, MediaService>();  

        services.AddDbContext<AppDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
