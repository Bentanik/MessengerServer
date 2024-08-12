using FluentValidation;
using FluentValidation.AspNetCore;
using MessengerServer.Src.Application;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.Settings;
using MessengerServer.Src.Infrastructure;
using MessengerServer.Src.WebApi.Validations.AuthencationValidations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MessengerServer.Src.WebApi;
public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
             .ConfigureApiBehaviorOptions(options =>
             {
                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var errors = context.ModelState.Values
                         .SelectMany(v => v?.Errors)
                         .Select(e => JsonSerializer.Deserialize<ErrorResponse>(e.ErrorMessage));
                     return new BadRequestObjectResult(new Result<object> { Error = 1, Message = "Missing value!", Data = errors });
                 };
             })
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                 options.JsonSerializerOptions.WriteIndented = true;
             });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddTransient<IValidatorInterceptor, UseCustomErrorModelInterceptor>();



        services.AddApplication();
        services.AddInfrastructure(configuration);

        services.Configure<EmailSetting>(configuration.GetSection(EmailSetting.SectionName));
        services.Configure<RedisSetting>(configuration.GetSection(RedisSetting.SectionName));
        services.Configure<JwtSetting>(configuration.GetSection(JwtSetting.SectionName));
        services.Configure<ClientSetting>(configuration.GetSection(ClientSetting.SectionName));



        return services;
    }
}