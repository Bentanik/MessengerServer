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

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<SignalRHub>("hub");

app.Run();
