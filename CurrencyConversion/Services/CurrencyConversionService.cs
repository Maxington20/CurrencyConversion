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

        public string BuildReponse(Currency currency, decimal amount, string toFromCad, string inputtedCurrency)
        {
            string newAmount = amount.ToString("0.0000");

            string convertedAmount = CalculateConvertedAmount(amount, currency.Rate);

            string begginingCurrency = toFromCad.ToLower() == "from" ? "CAD" : inputtedCurrency;
            string endingCurrency = toFromCad.ToLower() == "from" ? inputtedCurrency : "CAD";

            string response = ($"{currency.Description} on {currency.Date} = {currency.Rate}.\n" +
                $"Converted amount: {newAmount} {begginingCurrency} = {convertedAmount} {endingCurrency}");

            return response;
        }
      
        public async Task <Currency> GetBOCRateAsync(string code, string toFrom, DateTime? date)
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
                var convertedDate = date?.ToString("yyyy'-'MM'-'dd");

                // if users pick specific date, just get the result for that one day
                uri = baseUri + "observations/" + codeInOrder + "?start_date=" + convertedDate + "&end_date=" + convertedDate;
            }

            var response = "";

            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.GetStringAsync(uri);
                }                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Having the following issue communicating with the Bank of Canada API: {e.Message}");
            }            

            var currency = CreateCurrencyObject(response, codeInOrder);

            return currency;
        }

        private Currency CreateCurrencyObject(string jsonResponse, string codeInOrder)
        {
            Currency currency = new Currency();
            try
            {
                dynamic record = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                var description = record["seriesDetail"][codeInOrder]["description"];
                var rDate = record["observations"][0]["d"];
                var rate = record["observations"][0][codeInOrder]["v"];

                currency.Description = description;
                currency.Rate = Convert.ToDecimal(rate);
                currency.Date = rDate;
            }
            catch(ArgumentOutOfRangeException)
            {
                Console.WriteLine("The Bank of Canada API has no records for the day selected");
            }
            catch(Exception)
            {
                Console.WriteLine($"Mistakes were made.....");
            }
            return currency;
        }

        private string CalculateConvertedAmount(decimal amount, decimal rate)
        {
            return (amount * rate).ToString("0.0000");
        }
    }
}
