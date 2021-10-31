using Microsoft.Extensions.DependencyInjection;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.TLExchangeRatesProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTlExchangeRatesProvider(this IServiceCollection services)
        {
            services.AddHttpClient(nameof(TlExchangeRatesProvider));
            services.AddScoped<IExchangeRatesProvider, TlExchangeRatesProvider>();
            return services;
        }

    }
}
