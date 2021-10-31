using Microsoft.Extensions.DependencyInjection;
using System;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.TLExchangeRatesProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTlExchangeRatesProvider(this IServiceCollection services)
        {
            services.AddHttpClient(nameof(TlExchangeRatesProvider)).ConfigureHttpClient(options =>
            {
                options.BaseAddress = new Uri("https://trainlinerecruitment.github.io/exchangerates/api/latest/", UriKind.Absolute);
            });

            services.AddScoped<IExchangeRatesProvider, TlExchangeRatesProvider>();
            return services;
        }

    }
}
