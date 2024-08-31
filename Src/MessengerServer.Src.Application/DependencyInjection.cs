using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerServer.Src.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationServices, AuthenticationServices>();
        services.AddScoped<IProfileUserServices, ProfileUserServices>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IMessageServices, MessageService>();
        return services;
    }
}
