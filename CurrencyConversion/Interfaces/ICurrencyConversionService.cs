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
        Task<Currency> GetBOCRate(string code,string toFrom, string date = null);

        Currency CreateCurrencyObject(string jsonResponse, string codeInOrder);

        Task<string> BuildReponse(Currency currency);
    }
}
