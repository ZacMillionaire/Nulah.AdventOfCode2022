using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventConsole
{
    internal class Day5 : BaseDay<string>
    {
        public override string Part1Test()
        {
            var input = new List<string>
            {
                "    [D]    ",
                "[N] [C]    ",
                "[Z] [M] [P]",
                " 1   2   3 ",
                "",
                "move 1 from 2 to 1",
                "move 3 from 1 to 3",
                "move 2 from 2 to 1",
                "move 1 from 1 to 2",
            };

            return ExecuteStacking(input);
        }

        public override string Part2Test()
        {
            throw new NotImplementedException();
        }

        public override string GetPart1Answer()
        {
            return ExecuteStacking(Input);
        }

        public override string GetPart2Answer()
        {
            throw new NotImplementedException();
        }

        private ParsedSupplyInstructions ParseCrateInput(IEnumerable<string> input)
        {
            var parsedSupplyInstructions = new ParsedSupplyInstructions();

            var crateDefintion = new List<string>();

            foreach (var line in input)
            {
                if (line == "")
                {
                    // An empty line is the end of the crate definition,
                    // so parse the crates we have
                    parsedSupplyInstructions.CrateLayout.AddRange(ParseCrateDefinition(crateDefintion));
                }
                else if (line[0] != 'm')
                {
                    // Crate definitions never start with an m, but can start with any number of whitespace or a [
                    // Collect these for parsing on the first new line we hit. This also collects the line containing
                    // the number of crate columns
                    crateDefintion.Add(line);
                }
                else
                {
                    parsedSupplyInstructions.Commands.Enqueue(ParseMoveInstruction(line));
                }
            }

            return parsedSupplyInstructions;
        }

        /// <summary>
        /// Returns a list of stacks representing the crate defintion given
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<Stack<char>> ParseCrateDefinition(IEnumerable<string> input)
        {
            var crateLines = new List<string>();
            var numberOfColumns = 0;

            foreach (var line in input)
            {
                // Creates are defined as [A-Z] so store this for reference later
                if (line.Contains("["))
                {
                    crateLines.Add(line);
                    continue;
                }
                if (line == "")
                {
                    // An empty line is the end of the crate definition
                    break;
                }

                numberOfColumns = ParseLineForCrateStacks(line);
            }


            var crateLayout = Enumerable.Range(0, numberOfColumns)
                .Select(x => new Stack<char>())
                .ToList();

            // Reverse the lines to go bottom to top as a stack is LIFO
            // and I'm too drunk to change the type or handle that in the loop below
            crateLines.Reverse();

            foreach (var crateLine in crateLines)
            {
                ParseCrateColumns(crateLine, crateLayout);
            }

            return crateLayout;
        }

        /// <summary>
        /// Returns the highest int from the line containing the column numbers for crates
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private int ParseLineForCrateStacks(string line)
        {
            return int.Parse(line.Split(" ")
                .Last(x => !string.IsNullOrWhiteSpace(x)));
        }

        /// <summary>
        /// parses a crate line for...crates, then pushes them into the appropriate stack bucket as appropriate
        /// </summary>
        /// <param name="line"></param>
        /// <param name="crateContainer"></param>
        private void ParseCrateColumns(string line, List<Stack<char>> crateContainer)
        {
            // Match all creates with a regex looking for a crate defintion: [A-Z], or a sequence of 3 white spaces where a crate could be
            var crateMatches = Regex.Matches(line, @"(?:\[([A-Z])\]|\s{3})\s?");

            for (var i = 0; i < crateContainer.Count; i++)
            {
                // Check if the match was an empty space, or a crate
                // Crates are any sequence with any non-whitespace character
                if (!string.IsNullOrWhiteSpace(crateMatches[i].Groups[1].Value))
                {
                    // Regex works on strings, so the value of the group is a string, even if its
                    // a single char. Strings are arrays of char, so the crate letter is the 0 index.
                    // Push it onto the column stack
                    crateContainer[i].Push(crateMatches[i].Groups[1].Value[0]);
                }
            }
        }

        /// <summary>
        /// Parses the number of crates to move, and what column to move them from and to from a given string
        /// matching
        /// <para>
        /// <c>move (\d+) from (\d+) to (\d+)</c>
        /// </para>
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private MoveCommand ParseMoveInstruction(string line)
        {
            var instructionMatch = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
            if (instructionMatch.Success)
            {
                return new MoveCommand(int.Parse(instructionMatch.Groups[1].Value), int.Parse(instructionMatch.Groups[2].Value), int.Parse(instructionMatch.Groups[3].Value));
            }

            throw new InvalidOperationException($"Regex was not a match on {line}");
        }

        private struct MoveCommand
        {
            public int Count;
            public int FromColumn;
            public int ToColumn;

            public MoveCommand(int count, int from, int to)
            {
                Count = count;
                FromColumn = from;
                ToColumn = to;
            }
        }

        private class ParsedSupplyInstructions
        {
            public List<Stack<char>> CrateLayout { get; private set; } = new();
            public Queue<MoveCommand> Commands { get; private set; } = new();

        }

        private string ExecuteStacking(List<string> input)
        {
            var parsedInput = ParseCrateInput(input);

            PerformCrateRearrangement(parsedInput);

            return GetTopOfEachCrateStack(parsedInput.CrateLayout);
        }

        /// <summary>
        /// Rearranges the crates in accordance to the parsed instructions
        /// </summary>
        /// <param name="parsedDetails"></param>
        private void PerformCrateRearrangement(ParsedSupplyInstructions parsedDetails)
        {
            foreach (var moveCommand in parsedDetails.Commands)
            {
                // Offset the source/destination columns as we track them using 0 index, but the instructions are 1 indexed
                var sourceColumn = parsedDetails.CrateLayout[moveCommand.FromColumn - 1];
                var destinationColumn = parsedDetails.CrateLayout[moveCommand.ToColumn - 1];

                for (var i = 0; i != moveCommand.Count; i++)
                {
                    var movingCrate = sourceColumn.Pop();
                    destinationColumn.Push(movingCrate);
                }
            }
        }

        private string GetTopOfEachCrateStack(List<Stack<char>> crateStacks)
        {
            var topCrates = new List<char>();

            foreach (var crateStack in crateStacks)
            {
                topCrates.Add(crateStack.Pop());
            }

            return string.Join(string.Empty, topCrates);
        }
    }
}
