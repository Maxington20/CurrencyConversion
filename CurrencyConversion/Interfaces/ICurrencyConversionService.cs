using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConversion.Models;

namespace CurrencyConversion.Interfaces
{
    public interface ICurrencyConversionService
    {
        Task<Currency> GetBOCRateAsync(string code,string toFrom, DateTime? date);

        public string BuildReponse(Currency currency, decimal amount, string toFromCad, string inputtedCurrency);
    }
}
