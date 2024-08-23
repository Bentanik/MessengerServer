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
        return services;
    }
}
