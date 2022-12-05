using System.Reflection;

namespace AdventConsole;

internal abstract class BaseDay<T> : IDay<T>
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

    public abstract T Part1Test();
    public abstract T Part2Test();
    public abstract T GetPart1Answer();
    public abstract T GetPart2Answer();
}

internal interface IDay<T>
{
    T Part1Test();
    T Part2Test();
    T GetPart1Answer();
    T GetPart2Answer();
}