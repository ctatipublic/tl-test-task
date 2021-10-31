using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TL.XR.Infrastructure.DomainModel;
using TL.XR.Infrastructure.Interfaces;

namespace TL.XR.Controllers
{
    [Route("/convert")]
    public class ConvertController : Controller
    {
        private readonly IConversionService _conversionService;

        public ConvertController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpGet]
        [Route("{sourceCurrency}")]
        public async Task<IActionResult> GetConvert(string sourceCurrency, string targetCurrency, decimal amount)
        {
            var result = await _conversionService.ConvertAsync(new CurrencyConversion
            {
                From = new AmountInCurrency { Amount = amount, Currency = sourceCurrency },
                To = new AmountInCurrency { Currency = targetCurrency },
            });

            return new OkObjectResult(result);
        }

    }
}
