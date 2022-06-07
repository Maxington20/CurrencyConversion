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
        public static bool IsDateValid(DateTime date)
        {
            DateTime minDate = DateTime.Parse("2017-01-01");          

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

            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            if(!acceptedCurrencyCodes.Contains(code))
            {
                return false;
            }

            return true;
        }
    }
}
