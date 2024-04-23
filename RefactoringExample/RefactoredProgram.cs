using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static RefactoringExample.OrginData;

namespace RefactoringExample
{
    internal class RefactoredProgram
    {
        public class StatementData
        {
            public string Customer;
            public PerformanceInfo[] Performances;


            public StatementData(string customer, PerformanceInfo[] performances)
            {
                Customer = customer;
                Performances = performances;
            }
        }


        public string Statement(InvoiceInfo invoice, Dictionary<string, PlayInfo> plays)
        {
            StatementData statementData = new StatementData(
                invoice.Customer,
                invoice.Performances);
            return RenderPlainText(statementData, plays);

        }

        public string RenderPlainText(StatementData data, Dictionary<string, PlayInfo> plays)
        {
            string result = $"Statement for {data.Customer}\n";

            foreach (var perf in data.Performances)
            {
                // print line for this order
                result += $"\t{PlayFor(perf).Name}: {AmountFor(perf) / 100} ({perf.Audience} seats)\n";
            }

            result += $"Amount owed is {TotalAmount() / 100}\n";
            result += $"You earned {TotalVolumeCredits()} credits\n";
            return result;

            float TotalAmount()
            {
                float result = 0;
                foreach (var perf in data.Performances)
                {
                    result += AmountFor(perf);
                }
                return result;
            }

            int TotalVolumeCredits()
            {
                int result = 0;
                foreach (var perf in data.Performances)
                {
                    // add volume credits
                    result += VolumeCreditsFor(perf);
                }
                return result;
            }

            //提炼函数，替代临时变量
            float AmountFor(PerformanceInfo aPerforence)
            {
                float result = 0;
                switch (PlayFor(aPerforence).Type)
                {
                    case PlayType.tragedy:
                        result = 40000;
                        if (aPerforence.Audience > 30)
                        {
                            result += 1000 * (aPerforence.Audience - 30);
                        }
                        break;
                    case PlayType.comedy:
                        result = 30000;
                        if (aPerforence.Audience > 20)
                        {
                            result += 10000 + 500 * (aPerforence.Audience - 20);
                        }
                        result += 300 * aPerforence.Audience;
                        break;
                    default:
                        throw new ArgumentException($"unknown Type: {PlayFor(aPerforence).Type}");
                }

                return result;
            }

            //以查询替代临时变量
            PlayInfo PlayFor(PerformanceInfo aPerformance)
            {
                return plays[aPerformance.PlayID];
            }

            int VolumeCreditsFor(PerformanceInfo aPerformance)
            {
                int result = 0;
                // add volume credits
                result += Math.Max(aPerformance.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if (PlayType.comedy == PlayFor(aPerformance).Type)
                    result += (int)Math.Floor((double)aPerformance.Audience / 5);

                return result;
            }
        }
    }
}
