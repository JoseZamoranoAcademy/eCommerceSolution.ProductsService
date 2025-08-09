using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.Validators;
using FluentValidation;


namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

            services.AddScoped<IProductService,BusinessLogicLayer.Services.ProductService>();

            return services;
        }
    }
}
