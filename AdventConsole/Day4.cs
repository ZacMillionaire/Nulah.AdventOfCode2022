using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventConsole
{
    internal class Day4 : BaseDay<int>
    {
        public override int Part1Test()
        {
            var input = new List<string>()
            {
                "2-4,6-8",
                "2-3,4-5",
                "5-7,7-9",
                "2-8,3-7",
                "6-6,4-6",
                "2-6,4-8",
            };

            return ScoreInput(input, RangeContainedIn);
        }

        public override int GetPart1Answer()
        {
            return ScoreInput(Input, RangeContainedIn);
        }

        public override int Part2Test()
        {
            var input = new List<string>()
            {
                "2-4,6-8",
                "2-3,4-5",
                "5-7,7-9",
                "2-8,3-7",
                "6-6,4-6",
                "2-6,4-8",
            };

            return ScoreInput(input, RangeOverlapsWith);
        }
        public override int GetPart2Answer()
        {
            return ScoreInput(Input, RangeOverlapsWith);
        }

        private int ScoreInput(IEnumerable<string> input, Func<int[], int[], int> scoreFunc)
        {
            var regex = @"(\d+)\-(\d+)\,(\d+)\-(\d+)";

            return input.Select(x => Regex.Match(x, regex))
                .Where(x => x.Success)
                .Select(x => scoreFunc(
                        GroupValueToPair(x.Groups, 1),
                        GroupValueToPair(x.Groups, 3)
                    )
                )
                .Sum();
        }

        private int[] GroupValueToPair(GroupCollection group, int skip) => group.Values.Skip(skip)
                .Take(2)
                .Select(x => int.Parse(x.Value))
                .ToArray();

        private int RangeContainedIn(int[] left, int[] right)
        {
            if (left[0] >= right[0] && left[1] <= right[1]
                || right[0] >= left[0] && right[1] <= left[1])
            {
                return 1;
            }

            return 0;
        }

        private int RangeOverlapsWith(int[] left, int[] right)
        {
            if (left[1] < right[0] || left[0] > right[1])
            {
                return 0;
            }

            return 1;
        }
    }
}
