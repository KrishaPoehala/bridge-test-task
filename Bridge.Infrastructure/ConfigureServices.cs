using Bridge.Application.Common.Interfaces;
using Bridge.Domain.Repositories;
using Bridge.Infrastructure.Options;
using Bridge.Infrastructure.Persistance;
using Bridge.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bridge.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection self, IConfiguration configuration)
    {
        self.AddDbContext<DogsContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"))
        );

        self.AddScoped<IDogRepository, DogRepository>();
        self.AddScoped<DogsContextInitializer>();
        self.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<DogsContext>());

        self.AddOptions<RateLimiterOptions>().BindConfiguration(nameof(RateLimiterOptions));

        self.AddOptions<PingMessageOptions>().BindConfiguration(nameof(PingMessageOptions));

        return self;
    }
}
