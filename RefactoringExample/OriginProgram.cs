using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static RefactoringExample.OrginData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RefactoringExample
{
    internal class OriginProgram
    {
        public string Statement(InvoiceInfo invoice, Dictionary<string, PlayInfo> plays)
        {
            float totalAmount = 0;
            int volumeCredits = 0;
            string result = $"Statement for {invoice.Customer}\n";
            NumberFormatInfo format = new NumberFormatInfo { CurrencyDecimalDigits = 2, CurrencyDecimalSeparator = ".", CurrencySymbol = "$" };
            foreach (var perf in invoice.Performances)
            {
                PlayInfo play = plays[perf.PlayID];
                float thisAmount = 0;
                switch (play.Type)
                {
                    case PlayType.tragedy:
                        thisAmount = 40000;
                        if (perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case PlayType.comedy:
                        thisAmount = 30000;
                        if (perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new ArgumentException($"unknown Type: {play.Type}");
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if (PlayType.comedy == play.Type)
                    volumeCredits += (int)Math.Floor((double)perf.Audience / 5);
                // print line for this order
                result += $"\t{play.Name}: {thisAmount / 100} ({perf.Audience} seats)\n";
                totalAmount += thisAmount;
            }
            result += $"Amount owed is {totalAmount / 100}\n";
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }

    }
}
