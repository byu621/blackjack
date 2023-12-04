using System.Text;

namespace blackjack;

public class StandTable
{
    private string[] headers = {"A", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
    private string[] rows = {"20", "19", "18", "17", "16", "15"};
    private decimal[,] table;
    private int[,] count;

    public StandTable()
    {
        table = new decimal[rows.Length, headers.Length];
        count = new int[rows.Length, headers.Length];
    }

    public void Add(Card dealerCard, Hand player, decimal ev)
    {
        if (player.Value == 21) throw new ArgumentException();
        int column = dealerCard.Value == 1 ? 0 : 11 - dealerCard.Value;
        int row = player.Value <= 16 ? 4 : 20 - player.Value;
        table[row, column] += ev; 
        count[row, column] += 1;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#");
        foreach(string header in headers)
        {
            stringBuilder.Append($",{header}");
        }
        stringBuilder.AppendLine();

        return stringBuilder.ToString();
    }
}