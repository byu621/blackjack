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
            string probability = new Hand(Shape.HARD, i, false, false, false).CalculateDTP().ToString();
            sb.AppendLine($"H{i},{probability}");
        }

        for (int i = 21; i >= 11; i--)
        {
            string probability = new Hand(Shape.SOFT, i, false, false, false).CalculateDTP().ToString();
            sb.AppendLine($"S{i},{probability}");
        }

        return sb.ToString();
    }
}