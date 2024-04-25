using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactoringExample.BuildTestFramework
{
    public class Province
    {
        string name;
        List<Producer> producers = new List<Producer>() { };

        /// <summary>
        /// 总产量
        /// </summary>
        int totalProduction;
        int demand;

        /// <summary>
        /// 采购价格
        /// </summary>
        float price;

        public Province(string name, List<Producer> producers, int demand, float price)
        {
            this.name = name;
            this.producers = producers;
            foreach (Producer producer in producers)
            {
                this.totalProduction += producer.Production;
            }
            this.demand = demand;
            this.price = price;
        }

        public string Name { get => name; }
        public List<Producer> Producers { get => producers; }
        public int TotalProduction { get => totalProduction; set => totalProduction = value; }
        public int Demand { get => demand; set => demand = value; }
        public float Price { get => price; set => price = value; }

        public int GetShortFall()
        {
            return this.demand - this.totalProduction;
        }

        /// <summary>
        /// 计算最小总成本
        /// （收购价-成本）*产量
        /// 从价格低的开始买，一直到需求满足，实现总成本最小
        /// </summary>
        /// <returns></returns>
        public float GetDemandCost()
        {
            float result = 0;
            //升序排列
            this.producers.Sort((a, b) => -(a.Cost - b.Cost));
            int remainDemand = demand;
            foreach (var item in producers)
            {
                float contribute = MathF.Min(remainDemand, item.Production);
                remainDemand -= item.Production;

                if (contribute == 0) return result;
                result += contribute * item.Cost;
            }
            return result;
        }


        public float GetDemandVaule()
        {
            return this.price * this.demand;
        }

        public int GetSatisfiedDemand()
        {
            return (int)MathF.Min(this.demand, this.totalProduction);
        }
    }
}
