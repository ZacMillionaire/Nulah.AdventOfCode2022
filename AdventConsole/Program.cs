namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Day3();

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

        static void Day3()
        {
            var d3 = new Day3();
            var test1 = d3.Part1Test();
            if (test1 != 157)
            {
                throw new InvalidDataException($"Calculated result did not match expectation: {test1} should be '157'");
            }

            var test2 = d3.Part2Test();
            if (test2 != 70)
            {
                throw new InvalidDataException($"Calculated result did not match expectation: {test2} should be '70'");
            }


            var part1Answer = d3.GetPart1Answer();
            var part2Answer = d3.GetPart2Answer();
        }
    }
}