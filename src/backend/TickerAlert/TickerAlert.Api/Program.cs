using TickerAlert.Api.Extensions;
using TickerAlert.Application;
using TickerAlert.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddCustomSwaggerGen()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddCustomCors(builder.Configuration)
    .AddControllers();

var app = builder.Build();

app.InitializeDatabase();
app.AddSwaggerIfDevelopment();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.AddHubEndpoints();
app.MapControllers();
app.Run();

public partial class Program { }