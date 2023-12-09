using System.Text;

namespace blackjack;

public class HitTable
{
    private readonly string[] _headers = {"A", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
    private readonly List<string> _rows; 
    private readonly decimal[,] _table;
    private readonly int[,] _count;

    public HitTable()
    {
        _rows = new();
        for (int i = 20; i >= 4; i--) _rows.Add($"H{i}");
        for (int i = 20; i >= 12; i--) _rows.Add($"S{i}");
        _table = new decimal[_rows.Count, _headers.Length];
        _count = new int[_rows.Count, _headers.Length];
    }

    public void Add(Card dealerCard, Hand player, decimal ev)
    {
        if (player.Value >= 21) throw new ArgumentException();
        if (player is { Shape: Shape.HARD, Value: <= 3 }) throw new ArgumentException();
        if (player is { Shape: Shape.SOFT, Value: <= 11 }) throw new ArgumentException();
        
        int column = dealerCard.Value == 1 ? 0 : 11 - dealerCard.Value;
        int row = player.Shape == Shape.HARD ? 20 - player.Value : 20 - player.Value + 17;
        _table[row, column] += ev; 
        _count[row, column] += 1;
    }

    public decimal Get(Card dealerCard, Hand player)
    {
        if (player.Value >= 21) throw new ArgumentException();
        if (player is { Shape: Shape.HARD, Value: <= 3 }) throw new ArgumentException();
        if (player is { Shape: Shape.SOFT, Value: <= 11 }) throw new ArgumentException();
        
        int column = dealerCard.Value == 1 ? 0 : 11 - dealerCard.Value;
        int row = player.Shape == Shape.HARD ? 20 - player.Value : 20 - player.Value + 17;
        return _count[row, column] == 0 ? _table[row, column] : _table[row, column]/_count[row, column];
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#");
        foreach(string header in _headers)
        {
            stringBuilder.Append($",{header}");
        }
        for (int i = 0; i < _rows.Count; i++)
        {
            stringBuilder.AppendLine();
            string row = _rows[i];
            string ace = CellToString(i, 0);
            string ten = CellToString(i, 1);
            string nine = CellToString(i, 2);
            string eight = CellToString(i, 3);
            string seven = CellToString(i, 4);
            string six = CellToString(i, 5);
            string five = CellToString(i, 6);
            string four = CellToString(i, 7);
            string three = CellToString(i, 8);
            string two = CellToString(i, 9);
            stringBuilder.Append($"{row},{ace},{ten},{nine},{eight},{seven},{six},{five},{four},{three},{two}");
        }

        return stringBuilder.ToString();
    }

    private string CellToString(int row, int column)
    {
        decimal ev = _table[row, column];
        int count = _count[row, column] == 0 ? 1 : _count[row, column];
        return (ev / count * 100).ToString("0.00") + "%";
    }
}