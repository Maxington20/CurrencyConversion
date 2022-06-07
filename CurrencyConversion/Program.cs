using System;
using System.Collections.Generic;
using CurrencyConversion.Services;
using CurrencyConversion.Models;
using System.Threading.Tasks;
using CurrencyConversion.Utility;

namespace CurrencyConversion
{
    public class Program
    {   
        static async Task Main(string[] args)
        {
            bool quit = false;

            Console.WriteLine("This console application will take a currency code, an optional date, and whether you want to convert to or from CAD, " +
                    "and will give you the proper exhcnage rate. Enter 0 at any time to exit.\n");

            while (!quit)
            {
                CurrencyConversionService converter = new CurrencyConversionService();

                List<string> codes = CurrencyCodes.BuildCodeList();

                string joinedCodes = CurrencyCodes.BuildCodeString(codes);

                // get the currency code from the user
                string inputedCurrency = "";

                bool validCode = false;

                while (!validCode)
                {
                    Console.WriteLine("Please input a currency code from these options: ({0})", joinedCodes);
                    inputedCurrency = Console.ReadLine().ToUpper();

                    if (inputedCurrency == "0")
                    {
                        ExitProgram();
                    }

                    validCode = Validator.IsCurrencyCodeValid(inputedCurrency);
                }

                // get the optional date from the user
                DateTime date;
                bool dateValid = false;
                string convertedDate = "";

                while (!dateValid)
                {
                    Console.WriteLine("Please enter a date in this format (yyyy-mm-dd), or just hit enter to use the most recent results:");

                    var inputDate = Console.ReadLine();

                    if (inputDate == "0")
                    {
                        ExitProgram();
                    }

                    if (String.IsNullOrEmpty(inputDate.Trim()))
                    {
                        dateValid = true;
                    }

                    if(DateTime.TryParse(inputDate.Trim(), out date))
                    {
                        if(Validator.IsDateValid(date))
                        {
                            convertedDate = date.ToString("yyyy'-'MM'-'dd");
                            dateValid = true;
                        }                        
                    }
                }

                // determine whether user wants conversion to or from CAD
                bool validSelection = false;
                string toFromCad = "";

                while (!validSelection)
                {
                    Console.WriteLine("Input FROM if you would like the conversion from CAD to {0}, or TO if you would like the conversion to be " +
                        "from {0} to CAD:", inputedCurrency);
                    toFromCad = Console.ReadLine().ToLower();
                    if (toFromCad == "from" || toFromCad.ToLower() == "to")
                    {
                        validSelection = true;
                    }
                }

                Currency currency;

                if(!String.IsNullOrEmpty(convertedDate))
                {
                    currency = await converter.GetBOCRate(inputedCurrency, toFromCad, convertedDate);
                }
                else
                {
                    currency = await converter.GetBOCRate(inputedCurrency, toFromCad);
                }
                            
                var response = await converter.BuildReponse(currency);

                Console.WriteLine(response + "\n");

                Console.WriteLine("\nIf you would like to do another conversion, type yes");
                string again = Console.ReadLine().ToLower();

                if(again != "yes")
                {
                    quit = true;
                }                             
            }

            ExitProgram();

            void ExitProgram()
            {
                Console.WriteLine("Thank you for using the Currency Converter\n");
                Environment.Exit(0);
            }
        }
    }
}
