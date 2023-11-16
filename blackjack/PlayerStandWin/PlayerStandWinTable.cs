using CsvHelper;
using System.Globalization;
using static blackjack.DealerTotalProbability;
using static blackjack.Probability;
using static blackjack.HandType;

namespace blackjack;

public class PlayerStandWinTable
{
    private readonly Dictionary<Hand, PlayerStandWinProbability> _dictionary = new();

    private readonly DealerTotalTable _dealerTotalTable;

    public PlayerStandWinTable(DealerTotalTable dealerTotalTable)
    {
        _dealerTotalTable = dealerTotalTable;
    }

    public void Compute()
    {
        Hand hand = new(HARD, 21);
        Probability dealer21 = _dealerTotalTable.Get(new Hand(SOFT, 11)).P21;
        Probability winOnDealerAce = dealer21.Inverse();

        _dictionary.Add(hand, new(hand, winOnDealerAce));
    }


    public void WriteToCsv()
    {
        using (var writer = new StreamWriter($"data\\PlayerStand.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerStandWinProbabilityMap>();

            csv.WriteRecords(_dictionary.Values);
        }
    }
}