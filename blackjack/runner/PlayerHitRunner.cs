using System.Text;

namespace blackjack;

public class PlayerHitRunner
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
        Dictionary<string, Hand> playerHands = new()
        {
            {"BJ", new Hand(Shape.SOFT, 21, false, false, true)},
            {"21", new Hand(Shape.SOFT, 21, false, false, false)},
            {"20", new Hand(Shape.HARD, 20, false, false, false)},
            {"19", new Hand(Shape.HARD, 19, false, false, false)},
            {"18", new Hand(Shape.HARD, 18, false, false, false)},
            {"17", new Hand(Shape.HARD, 17, false, false, false)},
            {"16", new Hand(Shape.HARD, 16, false, false, false)},
        };

        StringBuilder sb = new();
        sb.AppendLine("#,A,10,9,8,7,6,5,4,3,2");

        foreach (KeyValuePair<string, Hand> kvp in playerHands)
        {
            sb.Append(kvp.Key);
            Hand playerHand = kvp.Value;
            foreach(Hand dealerHand in dealerHands)
            {
                sb.Append($",{playerHand.CalculatePHEV(dealerHand) * 100:0.00}%");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}