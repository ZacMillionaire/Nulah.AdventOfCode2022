namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Day1();

            Console.ReadKey();
        }

        static void Day1()
        {
            var d1 = new Day1();
            var largest = d1.GetLargest();
            var top3Total = d1.GetTop3Total();
        }
    }
}