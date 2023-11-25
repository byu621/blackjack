using System.Text;

namespace blackjack;

public static class PlayerStandRunner
{
    public static void Print()
    {
        System.Console.WriteLine(ToStringRepresentation());
    }

    public static void WriteToCsv()
    {
        string filePath = "data/PlayerStandEv.csv";
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
            string key = playerHand.Blackjack ? "BJ" : $"{playerHand.Value.ToString()}";
            sb.Append(key);
            foreach(Hand dealerHand in dealerHands)
            {
                sb.Append($",{playerHand.CalculatePSEV(dealerHand) * 100:0.00}%");
            }

            sb.AppendLine();
        }


        return sb.ToString();
    }
}