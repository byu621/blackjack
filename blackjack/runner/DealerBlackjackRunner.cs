using System.Text;

namespace blackjack;

public static class DealerBlackjackRunner
{
    public static void WriteToCsv()
    {
        string filePath = "data/DealerBlackjack.csv";
        string content = ToStringRepresentation();
        using StreamWriter writer = new StreamWriter(filePath);
        writer.Write(content);
    }
    
    public static string ToStringRepresentation()
    {
        List<Hand> playerHands = new()
        {
            new Hand(Shape.SOFT, 21, false, false, true),
            new Hand(Shape.HARD, 10, false, false, false)
        };
        
        StringBuilder sb = new();
        sb.AppendLine("#,ev");

        foreach (Hand playerHand in playerHands)
        {
            string key = playerHand.Blackjack ? "BJ" : "no";
            decimal ev = playerHand.CalculateDealerBJEV();
            sb.Append(key);
            sb.Append($",{ev * 100:0.00}%");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}