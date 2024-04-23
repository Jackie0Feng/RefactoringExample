namespace RefactoringExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OrginData data = new OrginData();
            OriginProgram originProgram = new OriginProgram();
            RefactoredProgram refactoredProgram = new RefactoredProgram();

            Console.WriteLine("originProgram----------------------------------\n");
            Console.WriteLine(originProgram.Statement(data.invoice, data.plays));
            Console.WriteLine("refactoredProgram----------------------------------\n");
            Console.WriteLine(refactoredProgram.Statement(data.invoice, data.plays));
        }
    }
}

