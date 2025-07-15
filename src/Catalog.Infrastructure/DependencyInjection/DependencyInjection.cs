using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Catalog.Infrastructure.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration config)
    {
        AddDbContext(services, config);
        AddRepositories(services);
        AddMassTransit(services);

        return services;
    }

    public static IHost ApplyMigrations(this IHost app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var servicesProvider = scope.ServiceProvider;
            var dbContext = servicesProvider.GetRequiredService<AppDbContext>();
            try
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                    dbContext.Database.Migrate();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Falha ao conectar com banco de dados | {ex.Message}");
            }
        }
        return app;
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgresString")));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddMassTransit(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });
    }
}
