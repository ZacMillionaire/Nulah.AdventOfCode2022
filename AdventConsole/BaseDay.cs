namespace AdventConsole;

internal class BaseDay
{
    protected List<string> Input = new();

    protected void ReadInput(string day)
    {
        foreach (var line in File.ReadAllLines($"Input/{day}/input.txt"))
        {
            Input.Add(line);
        }
    }
}