using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
            //RunDay<Day5, string>("CMZ", "MCD");
            RunDay<Day6, int[]>(new[] { 7, 5, 6, 10, 11 }, new int[] { 19, 23, 23, 29, 26 });
            RunDay<Day7, long>(95437, -1);

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
            CheckEquality(test1, test1ExpectedResult);
            Console.WriteLine($"Test 1 passed");

            var test2 = day.Part2Test();
            CheckEquality(test2, test2ExpectedResult);
            Console.WriteLine($"Test 2 passed");

            var part1Answer = day.GetPart1Answer();
            Console.WriteLine($"Part 1 Answer: {OutputAnswer(part1Answer)}");

            var part2Answer = day.GetPart2Answer();
            Console.WriteLine($"Part 2 Answer: {OutputAnswer(part2Answer)}");

            Console.ResetColor();
        }

        static void CheckEquality<T>(T left, T right)
        {
            if (typeof(T).IsArray)
            {
                if (left is int[] l && right is int[] r && !l.SequenceEqual(r))
                {
                    throw new InvalidDataException($"Calculated result did not match expectation: {string.Join(",", l)} should be '{string.Join(",", r)}'");
                }
            }
            else
            {
                if (!EqualityComparer<T>.Default.Equals(left, right))
                {
                    throw new InvalidDataException($"Calculated result did not match expectation: {left} should be '{right}'");
                }
            }
        }

        static string OutputAnswer<T>(T answer)
        {
            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer));
            }

            if (typeof(T).IsArray)
            {
                var sb = new StringBuilder();
                foreach (var b in (IList)answer)
                {
                    sb.Append(b).Append(",");
                }
                return sb.ToString();
            }

            return answer.ToString()!;
        }
    }
}