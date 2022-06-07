using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrencyConversion.Interfaces;
using CurrencyConversion.Models;
using Newtonsoft.Json;

namespace CurrencyConversion.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        public const string baseUri = "https://www.bankofcanada.ca/valet/";        

        HttpClient client = new HttpClient();

        public Task <string> BuildReponse(Currency currency)
        {            
            if (String.IsNullOrEmpty(currency.Description))
            {
                throw new ArgumentNullException("description");
            }   
            
            if (String.IsNullOrEmpty(currency.Rate))
            {
                throw new ArgumentNullException("rate");
            }
            
            string response = ($"{currency.Description} on {currency.Date} was {currency.Rate}");

            return Task.FromResult(response);
        }

        public Currency CreateCurrencyObject(string jsonResponse, string codeInOrder)
        {
            dynamic record = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            var description = record["seriesDetail"][codeInOrder]["description"];
            var rDate = record["observations"][0]["d"];
            var rate = record["observations"][0][codeInOrder]["v"];

            Currency currency = new Currency
            {
                Description = description,
                Rate = rate,
                Date = rDate,
            };

            return currency;
        }

        public async Task <Currency> GetBOCRate(string code, string toFrom, string date = null)
        {
            string uri;
            string codeInOrder = "FX";

            if(toFrom == "from")
            {
                codeInOrder = ($"{codeInOrder}CAD{code}");
            }
            else
            {
                codeInOrder = ($"{codeInOrder}{code}CAD");
            }

            if(date == null)
            {
                // if no date is specififed, get the most recent result
                uri = baseUri + "observations/" + codeInOrder + "?recent=1";
            }
            else
            {
                // if users pick specific date, just get the result for that one day
                uri = baseUri + "observations/" + codeInOrder + "?start_date=" + date + "&end_date=" + date;
            }

            var response = await client.GetStringAsync(uri);

            var currency = CreateCurrencyObject(response, codeInOrder);

            return currency;
        }       
    }
}
