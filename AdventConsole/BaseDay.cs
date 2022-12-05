using System.Reflection;

namespace AdventConsole;

internal abstract class BaseDay : IDay
{
    protected List<string> Input = new();

    protected BaseDay()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        foreach (var line in File.ReadAllLines($"Input/{GetType().Name}/input.txt"))
        {
            Input.Add(line);
        }
    }
    public abstract int Part1Test();
    public abstract int Part2Test();
    public abstract int GetPart1Answer();
    public abstract int GetPart2Answer();
}

internal interface IDay
{
    int Part1Test();
    int Part2Test();
    int GetPart1Answer();
    int GetPart2Answer();
}