namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        System.Console.WriteLine("Hello");
        Dictionary<Hand, int> dictionary = new();
        dictionary.Add(new (Shape.SOFT, 10, false, false, false), 1);

        bool contains = dictionary.ContainsKey(new Hand(Shape.SOFT, 10, false, false, false));
        System.Console.WriteLine(contains);
    }
}