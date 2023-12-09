using System.Text;

namespace blackjack;

public class StandTable
{
    private string[] headers = {"A", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
    private string[] rows = {"20", "19", "18", "17", "16"}; 
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
    
    public decimal Get(Card dealerCard, Hand player)
    {
        int column = dealerCard.Value == 1 ? 0 : 11 - dealerCard.Value;
        int row = player.Value <= 16 ? 4 : 20 - player.Value;
        return count[row, column] == 0 ? table[row, column] : table[row, column] / count[row, column];
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#");
        foreach(string header in headers)
        {
            stringBuilder.Append($",{header}");
        }
        for (int i = 0; i < rows.Length; i++)
        {
            stringBuilder.AppendLine();
            string row = rows[i];
            string ace = (table[i,0] / count[i,0] * 100).ToString("0.00") + "%";
            string ten = (table[i,1] / count[i,1] * 100).ToString("0.00") + "%";
            string nine = (table[i,2] / count[i,2] * 100).ToString("0.00") + "%";
            string eight = (table[i,3] / count[i,3] * 100).ToString("0.00") + "%";
            string seven = (table[i,4] / count[i,4] * 100).ToString("0.00") + "%";
            string six = (table[i,5] / count[i,5] * 100).ToString("0.00") + "%";
            string five = (table[i,6] / count[i,6] * 100).ToString("0.00") + "%";
            string four = (table[i,7] / count[i,7] * 100).ToString("0.00") + "%";
            string three = (table[i,8] / count[i,8] * 100).ToString("0.00") + "%";
            string two = (table[i,9] / count[i,9] * 100).ToString("0.00") + "%";
            stringBuilder.Append($"{row},{ace},{ten},{nine},{eight},{seven},{six},{five},{four},{three},{two}");
        }

        return stringBuilder.ToString();
    }
}