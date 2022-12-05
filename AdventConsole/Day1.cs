using System.Security.Cryptography.X509Certificates;

namespace AdventConsole;

internal class Day1 : BaseDay<int>
{

    private List<int> GetCalorieCount(IEnumerable<string> input)
    {
        var totals = new List<int>();

        var count = 0;

        foreach (var line in input)
        {
            if (line != string.Empty)
            {
                count += int.Parse(line);
            }
            else
            {
                totals.Add(count);
                count = 0;
            }
        }

        return totals;
    }

    public int GetLargest(IEnumerable<int> calorieSummary)
    {
        return calorieSummary.Max();
    }

    public int GetTop3Total(IEnumerable<int> calorieSummary)
    {
        return calorieSummary.OrderByDescending(x => x)
            .Take(3)
            .Sum();
    }

    public override int Part1Test()
    {
        var input = new List<string>
        {
            "1000",
            "2000",
            "3000",
            "",
            "4000",
            "",
            "5000",
            "6000",
            "",
            "7000",
            "8000",
            "9000",
            "",
            "10000",
            "",
        };

        var counts = GetCalorieCount(input);
        return GetLargest(counts);
    }

    public override int Part2Test()
    {
        var input = new List<string>
        {
            "1000",
            "2000",
            "3000",
            "",
            "4000",
            "",
            "5000",
            "6000",
            "",
            "7000",
            "8000",
            "9000",
            "",
            "10000",
            ""
        };

        var counts = GetCalorieCount(input);
        return GetTop3Total(counts);
    }

    public override int GetPart1Answer()
    {
        var counts = GetCalorieCount(Input);
        return GetLargest(counts);
    }

    public override int GetPart2Answer()
    {
        var counts = GetCalorieCount(Input);
        return GetTop3Total(counts);
    }
}