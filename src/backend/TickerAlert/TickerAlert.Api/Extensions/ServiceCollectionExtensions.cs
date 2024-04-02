namespace TickerAlert.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("AllowedOrigins")
            .Get<Dictionary<string, string>>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins(allowedOrigins.Values.ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}