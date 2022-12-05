namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //RunDay<Day1>(24000, 45000);
            //RunDay<Day2>(15, 12);
            //RunDay<Day3>(157, 70);
            //RunDay<Day4>(2, 4);

            Console.ReadKey();
        }

        static void RunDay<T>(int test1ExpectedResult, int test2ExpectedResult)
            where T : BaseDay, new()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Running {typeof(T).Name}");
            Console.ResetColor();

            var day = Activator.CreateInstance<T>();
            var test1 = day.Part1Test();
            if (test1 != test1ExpectedResult)
            {
                throw new InvalidDataException($"Calculated result did not match expectation: {test1} should be '{test1ExpectedResult}'");
            }
            Console.WriteLine($"Test 1 passed");

            var test2 = day.Part2Test();
            if (test2 != test2ExpectedResult)
            {
                throw new InvalidDataException($"Calculated result did not match expectation: {test2} should be '{test2ExpectedResult}'");
            }
            Console.WriteLine($"Test 2 passed");

            var part1Answer = day.GetPart1Answer();
            Console.WriteLine($"Part 1 Answer: {part1Answer}");
            var part2Answer = day.GetPart2Answer();
            Console.WriteLine($"Part 2 Answer: {part2Answer}");
            Console.ResetColor();
        }
    }
}