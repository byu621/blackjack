using System.Text;

namespace blackjack;

public class StandTable
{
    private readonly string[] _headers = {"A", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
    private readonly string[] _rows = {"20", "19", "18", "17", "16"}; 
    private readonly decimal[,] _table;
    private readonly int[,] _count;

    public StandTable()
    {
        _table = new decimal[_rows.Length, _headers.Length];
        _count = new int[_rows.Length, _headers.Length];
    }

    public void Add(Card dealerCard, Hand player, decimal ev)
    {
        if (player.Value == 21) throw new ArgumentException();
        int column = dealerCard.Value == 1 ? 0 : 11 - dealerCard.Value;
        int row = player.Value <= 16 ? 4 : 20 - player.Value;
        _table[row, column] += ev; 
        _count[row, column] += 1;
    }
    
    public decimal Get(Card dealerCard, Hand player)
    {
        int column = dealerCard.Value == 1 ? 0 : 11 - dealerCard.Value;
        int row = player.Value <= 16 ? 4 : 20 - player.Value;
        return _count[row, column] == 0 ? _table[row, column] : _table[row, column] / _count[row, column];
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#");
        foreach(string header in _headers)
        {
            stringBuilder.Append($",{header}");
        }
        for (int i = 0; i < _rows.Length; i++)
        {
            stringBuilder.AppendLine();
            string row = _rows[i];
            string ace = (_table[i,0] / _count[i,0] * 100).ToString("0.00") + "%";
            string ten = (_table[i,1] / _count[i,1] * 100).ToString("0.00") + "%";
            string nine = (_table[i,2] / _count[i,2] * 100).ToString("0.00") + "%";
            string eight = (_table[i,3] / _count[i,3] * 100).ToString("0.00") + "%";
            string seven = (_table[i,4] / _count[i,4] * 100).ToString("0.00") + "%";
            string six = (_table[i,5] / _count[i,5] * 100).ToString("0.00") + "%";
            string five = (_table[i,6] / _count[i,6] * 100).ToString("0.00") + "%";
            string four = (_table[i,7] / _count[i,7] * 100).ToString("0.00") + "%";
            string three = (_table[i,8] / _count[i,8] * 100).ToString("0.00") + "%";
            string two = (_table[i,9] / _count[i,9] * 100).ToString("0.00") + "%";
            stringBuilder.Append($"{row},{ace},{ten},{nine},{eight},{seven},{six},{five},{four},{three},{two}");
        }

        return stringBuilder.ToString();
    }
    
    public void WriteToFile()
    {
        using StreamWriter writer = new StreamWriter("data/stand.csv", false);
        writer.Write(ToString());
    }
}