using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactoringExample.BuildTestFramework
{
    public class ProvinceProgram
    {
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

        static void ProvinceMain(string[] args)
        {
            ProvinceProgram program = new ProvinceProgram();

        }
    }
}
