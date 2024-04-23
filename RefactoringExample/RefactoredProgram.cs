using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static RefactoringExample.OrginData;
using static RefactoringExample.RefactoredProgram.StatementData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RefactoringExample
{
    internal class RefactoredProgram
    {
        public class StatementData
        {
            public string Customer;
            public EnrichPerformanceInfo[] EnrichPerformances;
            public float TotalAmount;
            public int TotalVolumeCredits;

            public class EnrichPerformanceInfo
            {
                public PerformanceInfo Performance;
                public PlayInfo Play;
                public float Amount;
                public int VolumeCredits;
            }

            public StatementData(string customer, EnrichPerformanceInfo[] enrichPerformances, float totalAmount, int totalVolumeCredits) : this(customer, enrichPerformances)
            {
                TotalAmount = totalAmount;
                TotalVolumeCredits = totalVolumeCredits;
            }

            public StatementData(string customer, EnrichPerformanceInfo[] performances)
            {
                Customer = customer;
                EnrichPerformances = performances;
            }
        }

        public string Statement(InvoiceInfo invoice, Dictionary<string, PlayInfo> plays)
        {
            return RenderPlainText(CreateStatementData(invoice, plays));
        }

        public StatementData CreateStatementData(InvoiceInfo invoice, Dictionary<string, PlayInfo> plays)
        {
            StatementData statementData = new StatementData(
                invoice.Customer,
                invoice.Performances.Select(performance => EnrichPerformance(performance)).ToArray()
         );
            statementData.TotalAmount = TotalAmount();
            statementData.TotalVolumeCredits = TotalVolumeCredits();

            return statementData;

            //改循环为管道操作
            EnrichPerformanceInfo EnrichPerformance(PerformanceInfo performance)
            {
                EnrichPerformanceInfo result = new EnrichPerformanceInfo();
                result.Performance = performance.Copy();
                result.Play = PlayFor(result.Performance);
                result.Amount = AmountFor(result.Performance);
                result.VolumeCredits = VolumeCreditsFor(result.Performance);
                return result;
            }

            //以查询替代临时变量
            PlayInfo PlayFor(PerformanceInfo aPerformance)
            {
                return plays[aPerformance.PlayID];
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

            float TotalAmount()
            {
                float result = 0;
                result = statementData.EnrichPerformances.Select(enrichPerf =>
                {
                    return AmountFor(enrichPerf.Performance);
                }).Sum();
                return result;
            }

            int TotalVolumeCredits()
            {
                int result = 0;
                result = statementData.EnrichPerformances.Select(enrichPerf =>
                {
                    return VolumeCreditsFor(enrichPerf.Performance);
                }).Sum();
                return result;
            }
        }

        public string RenderPlainText(StatementData data)
        {
            string result = $"Statement for {data.Customer}\n";

            foreach (var enrichPerf in data.EnrichPerformances)
            {
                // print line for this order
                result += $"\t{enrichPerf.Play.Name}: {enrichPerf.Amount / 100} ({enrichPerf.Performance.Audience} seats)\n";
            }

            result += $"Amount owed is {data.TotalAmount / 100}\n";
            result += $"You earned {data.TotalVolumeCredits} credits\n";
            return result;
        }
    }
}
