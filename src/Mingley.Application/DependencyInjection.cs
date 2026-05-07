using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Mingley.Application.Mappings;

namespace Mingley.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}
