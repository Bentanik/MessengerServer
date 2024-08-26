using MessengerServer.Src.WebApi;
using MessengerServer.Src.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services
        .AddWebApi(builder.Configuration)
        .AddAuthenticationAndAuthorization(builder.Configuration);            

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("hub/notification");

app.Run();
