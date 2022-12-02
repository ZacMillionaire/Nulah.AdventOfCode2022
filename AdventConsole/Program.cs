namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Console.ReadKey();
        }

        static void Day1()
        {
            var d1 = new Day1();
            d1.ReadInput(@"E:\\C#\\Nulah\\Nulah.AdventOfCode2022\\Day1Input.txt");

            var largest = d1.GetLargest();
            var top3Total = d1.GetTop3Total();
        }
    }

    internal class Day1
    {
        private List<int> _totals = new List<int>();
        public void ReadInput(string fileLocation)
        {
            var count = 0;

            foreach (var line in File.ReadAllLines(fileLocation))
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
}