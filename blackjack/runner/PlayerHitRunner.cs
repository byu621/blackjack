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
        };

        for (int i = 21; i >= 4; i--)
        {
            playerHands.Add(new Hand(Shape.HARD, i, false, false, false));
        }
        
        for (int i = 21; i >= 13; i--)
        {
            playerHands.Add(new Hand(Shape.SOFT, i, false, false, false));
        }

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