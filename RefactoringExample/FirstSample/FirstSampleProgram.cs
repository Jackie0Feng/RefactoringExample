using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactoringExample.FirstSample
{
    internal class FirstSampleProgram
    {
        public static void FirstSampleMain()
        {
            OrginData data = new OrginData();
            OriginProgram originProgram = new OriginProgram();
            RefactoredProgram refactoredProgram = new RefactoredProgram();

            Console.WriteLine("originProgram----------------------------------\n");
            Console.WriteLine(originProgram.StatementMain(data.invoice, data.plays));
            Console.WriteLine("refactoredProgram----------------------------------\n");
            Console.WriteLine(refactoredProgram.StatementMain(data.invoice, data.plays));
        }
    }
}
