using CsvHelper;
using System.Globalization;
using static blackjack.HandType;
using static blackjack.Probability;

namespace blackjack;

public class PlayerStandWinTable
{
    private readonly List<PlayerStandWinProbability> _list = new();
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

        foreach(Hand dealerHand in dealerHands)
        {
            Probability notLose = Zero;

            notLose += _dealerTotalTable.Get(dealerHand).P21;
            _list.Add(new(21, dealerHand, One - notLose));

            notLose += _dealerTotalTable.Get(dealerHand).P20;
            _list.Add(new(20, dealerHand, One - notLose));

            notLose += _dealerTotalTable.Get(dealerHand).P19;
            _list.Add(new(19, dealerHand, One - notLose));

            notLose += _dealerTotalTable.Get(dealerHand).P18;
            _list.Add(new(18, dealerHand, One - notLose));

            notLose += _dealerTotalTable.Get(dealerHand).P17;
            _list.Add(new(17, dealerHand, One - notLose));

            _list.Add(new(16, dealerHand, One - notLose));
        }

        Console.WriteLine(_list.Count);
    }

    public void WriteToCsv()
    {
        List<PlayerStandWinDisplay> list = new();
        for (int i = 0; i < 6; i++)
        {
            PlayerStandWinDisplay playerStandWinDisplay = new();
            playerStandWinDisplay.PlayerTotal = _list[i].PlayerTotal;
            playerStandWinDisplay.DealerA = _list[i].Win;
            playerStandWinDisplay.Dealer10 = _list[i + 6].Win;
            playerStandWinDisplay.Dealer9 = _list[i + 12].Win;
            playerStandWinDisplay.Dealer8 = _list[i + 18].Win;
            playerStandWinDisplay.Dealer7 = _list[i + 24].Win;
            playerStandWinDisplay.Dealer6 = _list[i + 30].Win;
            playerStandWinDisplay.Dealer5 = _list[i + 36].Win;
            playerStandWinDisplay.Dealer4 = _list[i + 42].Win;
            playerStandWinDisplay.Dealer3 = _list[i + 48].Win;
            playerStandWinDisplay.Dealer2 = _list[i + 54].Win;

            list.Add(playerStandWinDisplay);
        }

        using (var writer = new StreamWriter($"data\\PlayerStandWin.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerStandWinDisplayMap>();
            csv.WriteRecords(list);
        }
    }
}