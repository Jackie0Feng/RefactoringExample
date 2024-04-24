using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static RefactoringExample.OrginData;
using static RefactoringExample.RefactoredProgram.StatementData;

namespace RefactoringExample
{
    internal class RefactoredProgram
    {
        public class PerformanceCalculator
        {
            public PerformanceInfo Performance;
            public PlayInfo Play;
            public virtual float Amount()
            {
                throw new NotImplementedException();
            }
            public virtual int VolumeCredits()
            {
                int result = 0;
                result += Math.Max(this.Performance.Audience - 30, 0);
                return result;
            }

            public PerformanceCalculator(PerformanceInfo aPerformance, PlayInfo aPlay)
            {
                this.Performance = aPerformance;
                this.Play = aPlay;
            }
        }
        public class TragedyCalculator : PerformanceCalculator
        {
            public TragedyCalculator(PerformanceInfo aPerformance, PlayInfo aPlay) : base(aPerformance, aPlay)
            {
            }

            public override float Amount()
            {
                float result;
                result = 40000;
                if (this.Performance.Audience > 30)
                {
                    result += 1000 * (this.Performance.Audience - 30);
                }
                return result;
            }
        }
        public class ComedyCalculator : PerformanceCalculator
        {
            public ComedyCalculator(PerformanceInfo aPerformance, PlayInfo aPlay) : base(aPerformance, aPlay)
            {
            }
            public override float Amount()
            {
                float result;
                result = 30000;
                if (this.Performance.Audience > 20)
                {
                    result += 10000 + 500 * (this.Performance.Audience - 20);
                }
                result += 300 * this.Performance.Audience;
                return result;
            }
            public override int VolumeCredits()
            {
                return base.VolumeCredits() + (int)Math.Floor((double)this.Performance.Audience / 5);
            }
        }

        /// <summary>
        /// 计算器工厂方法，用来实现多态+一个switch替代多个switch
        /// </summary>
        /// <param name="aPerformance"></param>
        /// <param name="aPlay"></param>
        /// <returns></returns>
        public PerformanceCalculator CreatePerformanceCalculator(PerformanceInfo aPerformance, PlayInfo aPlay)
        {
            switch (aPlay.Type)
            {
                case PlayType.tragedy:
                    return new TragedyCalculator(aPerformance, aPlay);
                case PlayType.comedy:
                    return new ComedyCalculator(aPerformance, aPlay);
                default:
                    throw new Exception();
            }
        }

        public class StatementData
        {
            public string Customer;
            public EnrichPerformanceInfo[] EnrichPerformances;
            public float TotalAmount;
            public int TotalVolumeCredits;

            public class EnrichPerformanceInfo : PerformanceInfo
            {
                //public PerformanceInfo Performance;
                public PlayInfo Play;
                public float Amount;
                public int VolumeCredits;

                public EnrichPerformanceInfo(PerformanceInfo performance) : base(performance) { }

            }

            public StatementData(string customer, EnrichPerformanceInfo[] performances)
            {
                Customer = customer;
                EnrichPerformances = performances;
            }

        }

        public string StatementMain(InvoiceInfo invoice, Dictionary<string, PlayInfo> plays)
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
                PerformanceCalculator calculator = CreatePerformanceCalculator(performance, PlayFor(performance));
                EnrichPerformanceInfo result = new EnrichPerformanceInfo(performance);
                result.Play = PlayFor(result);
                result.Amount = calculator.Amount();
                result.VolumeCredits = calculator.VolumeCredits();
                return result;
            }

            //以查询替代临时变量
            PlayInfo PlayFor(PerformanceInfo aPerformance)
            {
                return plays[aPerformance.PlayID];
            }

            //提炼函数，替代临时变量
            float AmountFor(PerformanceInfo aPerformance)
            {
                return CreatePerformanceCalculator(aPerformance, PlayFor(aPerformance)).Amount();
            }

            int VolumeCreditsFor(PerformanceInfo aPerformance)
            {
                return CreatePerformanceCalculator(aPerformance, PlayFor(aPerformance)).VolumeCredits(); ;
            }

            float TotalAmount()
            {
                float result = 0;
                result = statementData.EnrichPerformances.Select(enrichPerf =>
                {
                    return AmountFor(enrichPerf);
                }).Sum();
                return result;
            }

            int TotalVolumeCredits()
            {
                int result = 0;
                result = statementData.EnrichPerformances.Select(enrichPerf =>
                {
                    return VolumeCreditsFor(enrichPerf);
                }).Sum();
                return result;
            }
        }

        public string RenderPlainText(StatementData data)
        {
            string result = $"StatementMain for {data.Customer}\n";

            foreach (var enrichPerf in data.EnrichPerformances)
            {
                // print line for this order
                result += $"\t{enrichPerf.Play.Name}: {enrichPerf.Amount / 100} ({enrichPerf.Audience} seats)\n";
            }

            result += $"Amount owed is {data.TotalAmount / 100}\n";
            result += $"You earned {data.TotalVolumeCredits} credits\n";
            return result;
        }
    }
}
