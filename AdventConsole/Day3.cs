using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventConsole
{
    internal class Day3 : BaseDay
    {
        public int Part1Test()
        {
            var input = new List<string>
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

        public int Part2Test()
        {
            var input = new List<string>
            {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg",
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw",
            };

            var matchingItems = new List<char>();

            for (var i = 0; i < input.Count; i += 3)
            {
                matchingItems.Add(GetBadgeFromBags(input.Skip(i).Take(3)));
            }

            return matchingItems.Select(GetPriorityForChar)
                .Sum();
        }

        /// <summary>
        /// Gets the badge from the 3 bags of items, assuming there is a common item across all 3 'bags' (strings) given
        /// </summary>
        /// <param name="threeBagsOfItems"></param>
        /// <returns></returns>
        private char GetBadgeFromBags(IEnumerable<string> threeBagsOfItems)
        {
            var elfTrio = threeBagsOfItems
                .Select(x => string.Join("", x.Distinct()))
                .ToList();

            var badge = GetBadgeFromBags(elfTrio[0].AsSpan(), elfTrio[1].AsSpan(), elfTrio[2].AsSpan());

            return badge;
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

        public int GetPart2Answer()
        {
            var matchingItems = new List<char>();
            for (var i = 0; i < Input.Count; i += 3)
            {
                matchingItems.Add(GetBadgeFromBags(Input.Skip(i).Take(3)));
            }
            var prioritySum = matchingItems.Select(GetPriorityForChar)
                .Sum();

            return prioritySum;
        }

        /// <summary>
        /// Returns the char that matches in both spans. Assumes there is only ever 1 matching char in both spans and returns on first match
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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

        private char GetBadgeFromBags(ReadOnlySpan<char> firstBag, ReadOnlySpan<char> secondBag, ReadOnlySpan<char> thirdBag)
        {
            foreach (var firstItem in firstBag)
            {
                foreach (var secondItem in secondBag)
                {
                    foreach (var thirdItem in thirdBag)
                    {
                        if (firstItem == secondItem && firstItem == thirdItem)
                        {
                            return firstItem;
                        }
                    }
                }
            }

            throw new NotImplementedException("Case where no matching char in either half not implemented");
        }

        /// <summary>
        /// Returns the priority of a char, a-z being 1-16, and A-Z being 27-52
        /// </summary>
        /// <param name="charToPrioritise"></param>
        /// <returns></returns>
        private int GetPriorityForChar(char charToPrioritise)
        {
            // If the char is a or above, reduce it to nearest 0
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
