namespace RefactoringExample.FirstSample
{
    internal class OrginData
    {
        public enum PlayType
        {
            tragedy,
            comedy
        }

        public struct PlayInfo
        {
            public string Name;
            public PlayType Type;

            public PlayInfo(string name, PlayType type)
            {
                Name = name;
                Type = type;
            }
        }

        public struct InvoiceInfo
        {
            public string Customer;
            public PerformanceInfo[] Performances;

            public InvoiceInfo(string customer, PerformanceInfo[] performances)
            {
                Customer = customer;
                Performances = performances;
            }
        }

        public class PerformanceInfo
        {
            public string PlayID;
            public int Audience;

            public PerformanceInfo(string playID, int audience)
            {
                PlayID = playID;
                Audience = audience;
            }

            public PerformanceInfo(PerformanceInfo performance)
            {
                PlayID = performance.PlayID;
                Audience = performance.Audience;
            }

            public PerformanceInfo()
            {

            }

            public PerformanceInfo Copy()
            {
                return MemberwiseClone() as PerformanceInfo;
            }
        }

        public Dictionary<string, PlayInfo> plays = new Dictionary<string, PlayInfo>()
        {
            { "hamlet",new PlayInfo("Hamlet",PlayType.tragedy)},
            { "as-like",new PlayInfo("As You Like It",PlayType.comedy)} ,
            { "othello",new PlayInfo( "Othello", PlayType.tragedy) }
        };

        public InvoiceInfo invoice = new InvoiceInfo(
            "BigCo",
            [
                new PerformanceInfo("hamlet",55),
                new PerformanceInfo("as-like",35),
                new PerformanceInfo("othello",40)
            ]
            );



    }
}
