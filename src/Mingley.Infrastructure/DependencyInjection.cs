using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mingley.Application.Interfaces;
using Mingley.Infrastructure.Persistence;
using Mingley.Infrastructure.Services;

namespace Mingley.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<MingleyDbContext>(options =>
            //options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Mingley.Infrastructure")));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDiscoverService, DiscoverService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
