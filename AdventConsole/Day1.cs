namespace AdventConsole;

internal class Day1 : BaseDay
{
    private List<int> _totals = new();

    public Day1()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        var count = 0;

        foreach (var line in Input)
        {
            if (line != string.Empty)
            {
                count += int.Parse(line);
            }
            else
            {
                _totals.Add(count);
                count = 0;
            }
        }
    }

    public int GetLargest()
    {
        return _totals.Max();
    }

    public int GetTop3Total()
    {
        return _totals.OrderByDescending(x => x)
            .Take(3)
            .Sum();
    }
}