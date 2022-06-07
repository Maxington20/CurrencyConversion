using System;
using System.Collections.Generic;
using CurrencyConversion.Services;
using CurrencyConversion.Models;
using System.Threading.Tasks;
using CurrencyConversion.Utility;
using CurrencyConversion.Interfaces;

namespace CurrencyConversion
{
    public class Program
    {   
        static async Task Main(string[] args)
        {
            bool quit = false;
           
            Console.WriteLine("This console application will take a currency code, an optional date, whether you want to convert to or from CAD, " +
                    "and an amount, and will give you the proper exhcnage rate. Enter 0 at any time to exit.\n");

            while (!quit)
            {
                ICurrencyConversionService converter = new CurrencyConversionService();

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

                // get the amount the user wants converted
                var validAmount = false;
                decimal amount = 0;
                while (!validAmount)
                {
                    Console.WriteLine("Amount to convert:");
                    var stringAmount = Console.ReadLine();

                    if (stringAmount == "0")
                    {
                        ExitProgram();
                    }

                    if (decimal.TryParse(stringAmount.Trim(), out amount))
                    {
                        if (Validator.IsAmountValid(amount))
                        {
                            validAmount = true;
                        }
                    }
                }               

                // determine whether user wants conversion to or from CAD
                bool validSelection = false;
                string toFromCad = "";

                while (!validSelection)
                {
                    Console.WriteLine("Input FROM to convert from CAD to {0}, or TO to convert " +
                        "from {0} to CAD:", inputedCurrency);

                    toFromCad = Console.ReadLine();

                    if(Validator.IsToFromInputValid(toFromCad))
                    {
                        validSelection = true;  
                    }                   
                }

                // get the optional date from the user                
                DateTime? dateToUse = null;
                bool dateValid = false;

                while (!dateValid)
                {
                    Console.WriteLine("Please enter a past date in this format (yyyy-mm-dd), or hit ENTER for most recent rate available:");

                    var inputDate = Console.ReadLine();

                    if (inputDate == "0")
                    {
                        ExitProgram();
                    }

                    if (String.IsNullOrEmpty(inputDate.Trim()))
                    {
                        dateValid = true;
                    }

                    DateTime date;

                    if (DateTime.TryParse(inputDate.Trim(), out date))
                    {
                        dateToUse = date;

                        if (Validator.IsDateValid(date))
                        {
                            dateValid = true;
                        }
                    }
                }

                Currency currency = await converter.GetBOCRateAsync(inputedCurrency, toFromCad, dateToUse);
                
                if(currency.Description != null)
                {
                    var response = converter.BuildReponse(currency, amount, toFromCad, inputedCurrency);

                    Console.WriteLine(response + "\n");
                }
                                
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
