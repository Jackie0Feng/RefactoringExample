using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactoringExample.BuildTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactoringExample.BuildTestFramework.Tests
{
    [TestClass()]
    public class ProvinceTests
    {
        public ProvinceTests()
        {
            //共享数据会因为不可控的修改会导致测试出BUG
            InitData();
        }
        public Province InitProvinceData()
        {
            Province provinceData = new Province(
                name: "Asia",
                producers: [new Producer( "Byzantium",10,9),
                                    new Producer("Attalia" ,12,10),
                                    new Producer("Sinope",10,6)],
                demand: 30,
                price: 20);

            foreach (var item in provinceData.Producers)
            {
                item.Province = provinceData;
            }

            return provinceData;
        }

        public Province InitData()
        {
            return InitProvinceData();
        }

        [TestMethod()]
        public void GetShortFallTest()
        {
            Province provinceData = InitData();
            int result = provinceData.ShortFall;
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        public void GetProfitTest()
        {
            Province provinceData = InitData();
            Assert.AreEqual(230, provinceData.Profit);
        }

        [TestMethod()]
        public void ProductionChangeTest()
        {
            Province provinceData = InitData();
            provinceData.Producers[0].Production = 20;
            Assert.AreEqual(-6, provinceData.ShortFall);
            Assert.AreEqual(292, provinceData.Profit);
        }
    }
}