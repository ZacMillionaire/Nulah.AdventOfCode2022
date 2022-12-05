using System.Collections.Generic;

namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //RunDay<Day1, int>(24000, 45000);
            //RunDay<Day2, int>(15, 12);
            //RunDay<Day3, int>(157, 70);
            //RunDay<Day4, int>(2, 4);
            RunDay<Day5, string>("CMZ", "MCD");

            Console.ReadKey();
        }

        static void RunDay<T, T2>(T2 test1ExpectedResult, T2 test2ExpectedResult)
            where T : BaseDay<T2>, new()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Running {typeof(T).Name}");
            Console.ResetColor();

            var day = Activator.CreateInstance<T>();
            var test1 = day.Part1Test();
            if (!EqualityComparer<T2>.Default.Equals(test1, test1ExpectedResult))
            {
                throw new InvalidDataException($"Calculated result did not match expectation: {test1} should be '{test1ExpectedResult}'");
            }
            Console.WriteLine($"Test 1 passed");

            var test2 = day.Part2Test();
            if (!EqualityComparer<T2>.Default.Equals(test2, test2ExpectedResult))
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