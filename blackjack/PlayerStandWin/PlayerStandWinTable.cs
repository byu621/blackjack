using CsvHelper;
using System.Globalization;
using static blackjack.HandType;
using static blackjack.Probability;

namespace blackjack;

public class PlayerStandWinTable
{
    private readonly Dictionary<int, PlayerStandWinProbability> _dictionary = new();

    private readonly DealerTotalTable _dealerTotalTable;

    public PlayerStandWinTable(DealerTotalTable dealerTotalTable)
    {
        _dealerTotalTable = dealerTotalTable;
    }

    public void Compute()
    {
        Hand[] dealerHands = {
            new Hand(SOFT, 11), new Hand(HARD, 10), new Hand(HARD, 9), new Hand(HARD, 8), new Hand(HARD, 7), new Hand(HARD, 6), new Hand(HARD, 5),  new Hand(HARD, 4),  new Hand(HARD, 3), new Hand(HARD, 2)
        };

        Probability notWin = Zero;

        notWin += _dealerTotalTable.Get(new Hand(SOFT, 11)).P21;
        _dictionary.Add(21, new(21, One - notWin));

        notWin += _dealerTotalTable.Get(new Hand(SOFT, 11)).P20;
        _dictionary.Add(20, new(20, One - notWin));

        notWin += _dealerTotalTable.Get(new Hand(SOFT, 11)).P19;
        _dictionary.Add(19, new(19, One - notWin));

        notWin += _dealerTotalTable.Get(new Hand(SOFT, 11)).P18;
        _dictionary.Add(18, new(18, One - notWin));

        notWin += _dealerTotalTable.Get(new Hand(SOFT, 11)).P17;
        _dictionary.Add(17, new(17, One - notWin));

        _dictionary.Add(16, new(16, One - notWin));
    }

    public void WriteToCsv()
    {
        using (var writer = new StreamWriter($"data\\PlayerStandWin.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerStandWinProbabilityMap>();

            csv.WriteRecords(_dictionary.Values);
        }
    }
}