using MessengerServer.Src.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddApplication();
        return services;
    }
}
