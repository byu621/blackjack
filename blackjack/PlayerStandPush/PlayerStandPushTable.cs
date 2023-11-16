using CsvHelper;
using System.Globalization;
using static blackjack.HandType;
using static blackjack.Probability;

namespace blackjack;

public class PlayerStandPushTable
{
    private readonly List<PlayerStandPushProbability> _list = new();
    private readonly DealerTotalTable _dealerTotalTable;

    public PlayerStandPushTable(DealerTotalTable dealerTotalTable)
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
            Probability push;

            push = _dealerTotalTable.Get(dealerHand).P21;
            _list.Add(new(21, dealerHand, push));

            push = _dealerTotalTable.Get(dealerHand).P20;
            _list.Add(new(20, dealerHand, push));

            push = _dealerTotalTable.Get(dealerHand).P19;
            _list.Add(new(19, dealerHand, push));

            push = _dealerTotalTable.Get(dealerHand).P18;
            _list.Add(new(18, dealerHand, push));

            push = _dealerTotalTable.Get(dealerHand).P17;
            _list.Add(new(17, dealerHand, push));

            _list.Add(new(16, dealerHand, Zero));
        }
    }

    public void WriteToCsv()
    {
        List<PlayerStandPushDisplay> displayList = new();
        for (int i = 0; i < 6; i++)
        {
            PlayerStandPushDisplay playerStandPushDisplay = new();
            playerStandPushDisplay.PlayerTotal = _list[i].PlayerTotal;
            playerStandPushDisplay.DealerA = _list[i].Push;
            playerStandPushDisplay.Dealer10 = _list[i + 6].Push;
            playerStandPushDisplay.Dealer9 = _list[i + 12].Push;
            playerStandPushDisplay.Dealer8 = _list[i + 18].Push;
            playerStandPushDisplay.Dealer7 = _list[i + 24].Push;
            playerStandPushDisplay.Dealer6 = _list[i + 30].Push;
            playerStandPushDisplay.Dealer5 = _list[i + 36].Push;
            playerStandPushDisplay.Dealer4 = _list[i + 42].Push;
            playerStandPushDisplay.Dealer3 = _list[i + 48].Push;
            playerStandPushDisplay.Dealer2 = _list[i + 54].Push;

            displayList.Add(playerStandPushDisplay);
        }

        using (var writer = new StreamWriter($"data\\PlayerStandPush.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerStandPushDisplayMap>();
            csv.WriteRecords(displayList);
        }
    }
}