using System.Text;

namespace blackjack;

public static class DealerTotalRunnner
{
    public static void Print()
    {
        System.Console.WriteLine(ToStringRepresentation());
    }

    public static void WriteToCsv()
    {
        string filePath = "data/DealerTotal.csv";
        string content = ToStringRepresentation();
        using StreamWriter writer = new StreamWriter(filePath);
        writer.Write(content);
    }

    private static string ToStringRepresentation()
    {
        StringBuilder sb = new();
        sb.AppendLine("#,P17,P18,P19,P20,P21,PBust");

        for (int i = 21; i >= 2; i--)
        {
            Hand hand = new Hand(Shape.HARD, i, false, false, false);
            decimal p17 = hand.CalculateDealerTotalProbability(false, 17);
            decimal p18 = hand.CalculateDealerTotalProbability(false, 18);
            decimal p19 = hand.CalculateDealerTotalProbability(false, 19);
            decimal p20 = hand.CalculateDealerTotalProbability(false, 20);
            decimal p21 = hand.CalculateDealerTotalProbability(false, 21);
            decimal pBust = hand.CalculateDealerTotalProbability(true, -1);
            sb.AppendLine($"H{i},{p17 * 100:0.00}%,{p18 * 100:0.00}%,{p19 * 100:0.00}%,{p20 * 100:0.00}%,{p21 * 100:0.00}%,{pBust * 100:0.00}%");
        }

        for (int i = 21; i >= 11; i--)
        {
            Hand hand = new Hand(Shape.SOFT, i, false, false, false);
            decimal p17 = hand.CalculateDealerTotalProbability(false, 17);
            decimal p18 = hand.CalculateDealerTotalProbability(false, 18);
            decimal p19 = hand.CalculateDealerTotalProbability(false, 19);
            decimal p20 = hand.CalculateDealerTotalProbability(false, 20);
            decimal p21 = hand.CalculateDealerTotalProbability(false, 21);
            decimal pBust = hand.CalculateDealerTotalProbability(true, -1);
            sb.AppendLine($"S{i},{p17 * 100:0.00}%,{p18 * 100:0.00}%,{p19 * 100:0.00}%,{p20 * 100:0.00}%,{p21 * 100:0.00}%,{pBust * 100:0.00}%");
        }

        return sb.ToString();
    }
}