using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // Add service to the IoC container
        // AutoMapper
        services.AddAutoMapper(cfg => { }, typeof(ProductAddRequestToProductMappingProfile).Assembly);

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

        services.AddScoped<IProductsService, ProductsService>();

        // Add RabbitMQ as a service
        services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();

        return services;
    }
}
