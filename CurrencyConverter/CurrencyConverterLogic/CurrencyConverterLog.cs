using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CurrencyConverterLogic
{
    public class CurrencyConverterLog
    {

        public IEnumerable<ExchangeRate> getExchangeRates(string content)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            var lines = deleteFirstLine(content);

            foreach (var line in lines)
            {

                var exchangeRate = new ExchangeRate()
                {
                    currency = line[0],
                    rate = decimal.Parse(line[1], CultureInfo.InvariantCulture),
                };
                exchangeRates.Add(exchangeRate);
            }
            return exchangeRates;
        }

        public IEnumerable<Product> getProducts(string content)
        {
            List<Product> products = new List<Product>();

            var lines = deleteFirstLine(content);

            foreach (var line in lines)
            {

                Product product = new Product()
                {
                    description = line[0],
                    currency = line[1],
                    price = decimal.Parse(line[2], CultureInfo.InvariantCulture),
                };
                products.Add(product);
            }
            return products;
        }

        public List<string[]> deleteFirstLine(string content)
        {
            var lines = content.Replace('\r', ' ').Split('\n');
            List<string[]> helperList = new List<string[]>();

            bool firstLine = true;
            foreach (var line in lines)
            {
                if (!firstLine)
                {
                    var curElements = line.Split(",");
                    if (curElements.Length > 1)
                        helperList.Add(curElements);
                }
                firstLine = false;
            }

            return helperList;
        }

        public decimal calcPrice(string product, string currency, IEnumerable<Product> products, IEnumerable<ExchangeRate> exchangeRates)
        {

            Product currentProduct = products.Where(p => p.description.Equals(product)).FirstOrDefault();
            decimal rateFactor = exchangeRates.Where(p => p.currency.Equals(currentProduct.currency)).FirstOrDefault().rate;

            decimal endPrice = currentProduct.price / rateFactor;
            return Math.Round(endPrice, 2);
        }
    }

}