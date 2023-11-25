using System.Text;

namespace blackjack;

public static class PlayerHitRunner
{
    public static void Print()
    {
        Console.WriteLine(ToStringRepresentation());
    }

    public static void WriteToCsv()
    {
        string filePath = "data/PlayerHitEv.csv";
        string content = ToStringRepresentation();
        using StreamWriter writer = new StreamWriter(filePath);
        writer.Write(content);
    }

    private static string ToStringRepresentation()
    {
        List<Hand> dealerHands = Hand.GetDealerStartingHands();
        List<Hand> playerHands = new()
        {
            new Hand(Shape.SOFT, 21, false, false, true),
            new Hand(Shape.HARD, 21, false, false, false),
            new Hand(Shape.HARD, 20, false, false, false),
            new Hand(Shape.HARD, 19, false, false, false),
            new Hand(Shape.HARD, 18, false, false, false),
            new Hand(Shape.HARD, 17, false, false, false),
            new Hand(Shape.HARD, 16, false, false, false),
        };

        StringBuilder sb = new();
        sb.AppendLine("#,A,10,9,8,7,6,5,4,3,2");

        foreach (Hand playerHand in playerHands)
        {
            string prefix = playerHand.Shape == Shape.SOFT ? "S" : "H";
            string key = playerHand.Blackjack ? "BJ" : $"{prefix}{playerHand.Value.ToString()}";
            sb.Append(key);
            foreach(Hand dealerHand in dealerHands)
            {
                sb.Append($",{playerHand.CalculatePHEV(dealerHand) * 100:0.00}%");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}