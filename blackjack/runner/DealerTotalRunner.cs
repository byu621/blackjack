namespace blackjack;

public static class DealerTotalRunnner
{
    public static void Print()
    {
        System.Console.WriteLine(new Hand(Shape.HARD, 16, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.HARD, 15, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.HARD, 14, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.HARD, 13, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.HARD, 12, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.HARD, 11, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.SOFT, 16, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.SOFT, 15, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.SOFT, 14, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.SOFT, 13, false, false, false).CalculateDTP());
        System.Console.WriteLine(new Hand(Shape.SOFT, 12, false, false, false).CalculateDTP());
    }
}