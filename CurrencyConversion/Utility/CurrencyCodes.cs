using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversion.Utility
{
    public class CurrencyCodes
    {
        // Used an enum. Easy to extend this app to include more currencies
        public enum IncludedCurrencyCodes
        {
            USD,
            EUR,
            JPY,
            GBP,
            AUD,
            CHF,
            CNY,
            HKD,
            MXN,
            INR,
        }

        public static List<string> BuildCodeList()
        {
           List<string> codeList = new List<string>();

           var codes = Enum.GetValues(typeof(IncludedCurrencyCodes));

           foreach(var code in codes)
            {
                codeList.Add(code.ToString());
            }

           return codeList;
        }

        public static string BuildCodeString(List<string> codeList)
        {
            return String.Join(",", codeList);
        }
    }
}
