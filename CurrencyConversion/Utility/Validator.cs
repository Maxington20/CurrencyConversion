using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CurrencyConversion.Utility.CurrencyCodes;

namespace CurrencyConversion.Utility
{
    public class Validator
    { 
        public enum ToFrom
        {
            TO,
            FROM,
        }
                
        public static bool IsDateValid(DateTime date)
        {
            // Played with postman, I believe this is the oldest record the api has data for
            DateTime minDate = DateTime.Parse("2017-01-03");          

            if (date >= DateTime.Now.AddDays(1))
            {
                return false;
            }      

            if (date <= minDate)
            {
                return false;
            }

            return true;
        }

        public static bool IsCurrencyCodeValid(string code)
        {
            // create empty list to store the codes
            List<string> acceptedCurrencyCodes = CurrencyCodes.BuildCodeList();        

            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            if(!acceptedCurrencyCodes.Contains(code))
            {
                return false;
            }

            return true;
        }

        public static bool IsToFromInputValid(string toFrom)
        {
            if (string.IsNullOrWhiteSpace(toFrom))
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(ToFrom), toFrom.ToUpper()))
            {
                return false;
            }

            return true;
        }

        public static bool IsAmountValid(decimal amount)
        {
            if(amount < 0)
            {
                return false;
            }   
            return true;
        }
    }
}
