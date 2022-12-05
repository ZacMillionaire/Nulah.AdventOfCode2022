using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventConsole
{
    internal class Day2 : BaseDay
    {
        private const int WinBonus = 6;
        private const int DrawBonus = 3;

        /// <summary>
        /// Determines the action to perform, based on the character input. Elves use A-C for Rock,Paper,Scissors respectively,
        /// the player uses X-Z for the same
        /// </summary>
        /// <param name="actionInput"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private Action ParseRockPaperScissorAction(char actionInput)
        {
            switch (actionInput)
            {
                case 'A':
                case 'X':
                    return Action.Rock;
                case 'B':
                case 'Y':
                    return Action.Paper;
                case 'C':
                case 'Z':
                    return Action.Scissors;
            }

            throw new NotSupportedException($"{actionInput} not supported");
        }

        /// <summary>
        /// Parses the expected outcome when treating X-Z in the strategy to not be an action, but a result to achieve.
        /// X for lose, Y for draw, Z for win
        /// </summary>
        /// <param name="actionInput"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private ExpectedOutcome ParseExpectedOutcome(char actionInput)
        {
            switch (actionInput)
            {
                case 'X':
                    return ExpectedOutcome.Lose;
                case 'Y':
                    return ExpectedOutcome.Draw;
                case 'Z':
                    return ExpectedOutcome.Win;
            }

            throw new NotSupportedException($"{actionInput} not supported");
        }

        /// <summary>
        /// Compares the 2 actions and returns the score based on the outcome of rock, paper, scissors
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private int ScoreResult(Action attacker, Action defender)
        {
            // The base score is the value of the defenders action
            var actionScoreBase = (int)defender;

            // win conditions based on rock paper scissors
            if (defender == Action.Rock && attacker == Action.Scissors
                || defender == Action.Scissors && attacker == Action.Paper
                || defender == Action.Paper && attacker == Action.Rock)
            {
                // Winning adds a bonus
                actionScoreBase += WinBonus;
            }

            // if equal its a draw
            if (attacker == defender)
            {
                // draw adds a smaller bonus
                actionScoreBase += DrawBonus;
            }

            // a loss is just the base value of the right action
            return actionScoreBase;
        }

        /// <summary>
        /// Given the attackers action and the outcome we need to produce to meet the condition, returns the appropriate
        /// action to result in a correct score
        /// </summary>
        /// <param name="attackerAction"></param>
        /// <param name="outcomeToProduce"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private Action ProduceOrchestratedResult(Action attackerAction, ExpectedOutcome outcomeToProduce)
        {
            // Lose outcomes mean we have to produce an action that would lose
            if (outcomeToProduce == ExpectedOutcome.Lose)
            {
                switch (attackerAction)
                {
                    case Action.Rock:
                        return Action.Scissors;
                    case Action.Paper:
                        return Action.Rock;
                    case Action.Scissors:
                        return Action.Paper;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attackerAction), attackerAction, null);
                }
            }

            // Win means we have to produce a win
            if (outcomeToProduce == ExpectedOutcome.Win)
            {
                switch (attackerAction)
                {
                    case Action.Rock:
                        return Action.Paper;
                    case Action.Paper:
                        return Action.Scissors;
                    case Action.Scissors:
                        return Action.Rock;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attackerAction), attackerAction, null);
                }
            }

            // And a draw means we return the same action
            return attackerAction;
        }

        private enum Action
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private enum ExpectedOutcome
        {
            /// <summary>
            /// Z
            /// </summary>
            Win,
            /// <summary>
            /// X
            /// </summary>
            Lose,
            /// <summary>
            /// Y
            /// </summary>
            Draw
        }

        private int GetGameOutcome(List<string> gameInput)
        {
            var runningTotal = 0;
            foreach (var line in gameInput)
            {
                var elfAction = ParseRockPaperScissorAction(line[0]);
                var myAction = ParseRockPaperScissorAction(line[2]);
                runningTotal += ScoreResult(elfAction, myAction);
            }

            return runningTotal;
        }
        private int GetOrchestratedGameOutcome(List<string> gameInput)
        {
            var runningTotal = 0;
            foreach (var line in gameInput)
            {
                var elfAction = ParseRockPaperScissorAction(line[0]);
                var myAction = ProduceOrchestratedResult(elfAction, ParseExpectedOutcome(line[2]));
                runningTotal += ScoreResult(elfAction, myAction);
            }

            return runningTotal;
        }

        public override int Part1Test()
        {
            var input = new List<string>
            {
                "A Y",
                "B X",
                "C Z",
            };
            return GetGameOutcome(input);
        }

        public override int Part2Test()
        {
            var input = new List<string>
            {
                "A Y",
                "B X",
                "C Z",
            };

            return GetOrchestratedGameOutcome(input);
        }

        public override int GetPart1Answer()
        {
            return GetGameOutcome(Input);
        }

        public override int GetPart2Answer()
        {
            return GetOrchestratedGameOutcome(Input);
        }
    }
}
