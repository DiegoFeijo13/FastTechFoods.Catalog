using Catalog.Application.Validators;
using FluentValidation;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        AddMediatr(services);
        AddValidation(services);
   
        return services;
    }
    private static void AddMediatr(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
        });

    }
    private static void AddValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ProdutoInputDTOValidator>();
        
    }
}
