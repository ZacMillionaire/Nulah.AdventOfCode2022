using System.Reflection;

namespace AdventConsole;

internal class BaseDay
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
}