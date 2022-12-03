using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventConsole
{
    internal class Day3 : BaseDay
    {
        /// <summary>
        /// Value to check against
        /// </summary>
        private readonly int _lowercaseCuttoff = 'a';

        public int Part1Test()
        {
            var input = new List<string>()
            {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg",
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw",
            };

            var matchingItems = new List<char>();

            foreach (var line in input)
            {
                var span = line.AsSpan();
                matchingItems.Add(GetFirstMatchingCharacter(span[0..(span.Length / 2)], span[(span.Length / 2)..span.Length]));
            }

            var prioritySum = matchingItems.Select(GetPriorityForChar)
                .Sum();

            return prioritySum;
        }

        public int GetPart1Answer()
        {
            var matchingItems = new List<char>();
            foreach (var line in Input)
            {
                var span = line.AsSpan();
                matchingItems.Add(GetFirstMatchingCharacter(span[0..(span.Length / 2)], span[(span.Length / 2)..span.Length]));
            }
            var prioritySum = matchingItems.Select(GetPriorityForChar)
                .Sum();

            return prioritySum;
        }

        private char GetFirstMatchingCharacter(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
        {
            foreach (var leftHalf in left)
            {
                foreach (var rightHalf in right)
                {
                    if (leftHalf == rightHalf)
                    {
                        return leftHalf;
                    }
                }
            }

            throw new NotImplementedException("Case where no matching char in either half not implemented");
        }

        private int GetPriorityForChar(char charToPrioritise)
        {
            if (charToPrioritise >= 'a')
            {
                // +1 as lowercase letter priority is 1-26
                return charToPrioritise - 'a' + 1;
            }

            // uppercase priority is 27-52
            return charToPrioritise - 'A' + 27;
        }
    }
}
