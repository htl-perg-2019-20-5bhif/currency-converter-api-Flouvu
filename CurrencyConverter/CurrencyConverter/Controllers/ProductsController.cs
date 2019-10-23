using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CurrencyConverterLogic;
using System.Net.Http;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly CurrencyConverterLog curConverter = new CurrencyConverterLog();

        public ProductsController(IHttpClientFactory factory)
        {
            httpClient = factory.CreateClient("currencyAPI");
        }

        [HttpGet]
        [Route("{product}/price")]
        public async Task<IActionResult> GetPrice(string product, [FromQuery] string targetCurrency)
        {
            var ratesString = await httpClient.GetStringAsync("ExchangeRates.csv");
            var productsString = await httpClient.GetStringAsync("Prices.csv");

            var exchangeRates = curConverter.getExchangeRates(ratesString);
            var products = curConverter.getProducts(productsString);

            decimal price = curConverter.calcPrice(product, targetCurrency, products, exchangeRates);
            return Ok(new EndPrice() { Price = price });
        }
    }
}
