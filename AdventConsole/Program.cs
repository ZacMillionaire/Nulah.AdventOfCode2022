namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Day2();

            Console.ReadKey();
        }

        static void Day1()
        {
            var d1 = new Day1();
            var largest = d1.GetLargest();
            var top3Total = d1.GetTop3Total();
        }

        static void Day2()
        {
            var d2 = new Day2();
            var strategyScore = d2.GetStrategyScore();
            var orchestratedScore = d2.CalculateRealStrategyScore();
        }
    }
}