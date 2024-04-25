using System.Globalization;
using static RefactoringExample.FirstSample.OrginData;

namespace RefactoringExample.FirstSample
{
    internal class OriginProgram
    {
        public string StatementMain(InvoiceInfo invoice, Dictionary<string, PlayInfo> plays)
        {
            float totalAmount = 0;
            int volumeCredits = 0;
            string result = $"StatementMain for {invoice.Customer}\n";
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
