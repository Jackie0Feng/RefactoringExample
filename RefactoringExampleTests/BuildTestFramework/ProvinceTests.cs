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
        ProvinceProgram program = new ProvinceProgram();
        Province province;

        public ProvinceTests()
        {
            province = program.InitProvinceData();
        }

        [TestMethod()]
        public void GetShortFallTest()
        {
            int result = province.GetShortFall();
            Assert.AreEqual(5, result);
        }
    }
}