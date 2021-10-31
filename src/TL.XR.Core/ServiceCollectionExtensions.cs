using Microsoft.Extensions.DependencyInjection;
using TL.XR.Core.Services;
using TL.XR.Infrastructure.Interfaces;
using TL.XR.TLExchangeRatesProvider;

namespace TL.XR.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTlXrCore(this IServiceCollection services)
        {
            services.AddScoped<IConversionService, ConversionService>();
            services.AddTlExchangeRatesProvider();
            return services;
        }
    }
}
