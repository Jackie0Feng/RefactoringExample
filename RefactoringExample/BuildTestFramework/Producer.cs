namespace RefactoringExample.BuildTestFramework
{
    public class Producer
    {
        private Province province;
        private string name;
        private int cost;

        /// <summary>
        /// 产量
        /// </summary>
        private int production;

        public Producer(string name, int cost, int production)
        {
            this.name = name;
            this.cost = cost;
            this.production = production;
        }

        public string Name { get => name; }
        public int Cost { get => cost; }
        public int Production
        {
            get => production;
            set
            {
                province.TotalProduction -= production;
                province.TotalProduction += value;
                production = value;
            }
        }
        public Province Province { get => province; set => province = value; }
    }
}