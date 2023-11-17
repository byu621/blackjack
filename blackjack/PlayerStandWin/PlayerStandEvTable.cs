using CsvHelper;
using System.Globalization;
using static blackjack.HandType;
using static blackjack.Probability;

namespace blackjack;

public class PlayerStandEvTable
{
    private readonly List<PlayerStandEvProbability> _list = new();
    private readonly DealerTotalTable _dealerTotalTable;

    public PlayerStandEvTable(DealerTotalTable dealerTotalTable)
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
            Probability dealerBlackjack = dealerHand.Equals(Hand.Ace) ? BlackjackGivenAce 
                                        : dealerHand.Equals(Hand.Ten) ? BlackjackGivenTen : Zero;
            Probability win;
            Probability push;
            Probability lose;
            DealerTotalProbability dealerProbability = _dealerTotalTable.Get(dealerHand);
            Ev ev;

            // BJ
            win = One - dealerBlackjack;
            lose = Zero;
            ev = Ev.Calc(win, lose);
            _list.Add(new("BJ", dealerHand, ev));

            //21
            push = dealerProbability.P21 - dealerBlackjack;
            lose = dealerBlackjack;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _list.Add(new("21", dealerHand, ev));

            //20
            push = dealerProbability.P20;
            lose = dealerProbability.P21;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _list.Add(new("20", dealerHand, ev));

            //19
            push = dealerProbability.P19;
            lose = dealerProbability.P21 + dealerProbability.P20;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _list.Add(new("19", dealerHand, ev));

            //18
            push = dealerProbability.P18;
            lose = dealerProbability.P21 + dealerProbability.P20 + dealerProbability.P19;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _list.Add(new("18", dealerHand, ev));

            //17
            push = dealerProbability.P17;
            lose = dealerProbability.P21 + dealerProbability.P20 + dealerProbability.P19 + dealerProbability.P18;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _list.Add(new("17", dealerHand, ev));

            //16
            push = Zero;
            lose = dealerProbability.P21 + dealerProbability.P20 + dealerProbability.P19 + dealerProbability.P18 + dealerProbability.P17;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _list.Add(new("16", dealerHand, ev));
        }
    }

    public void WriteToCsv()
    {
        Console.WriteLine(_list.Count);
        List<PlayerStandEvDisplay> displayList = new();
        for (int i = 0; i < 7; i++)
        {
            PlayerStandEvDisplay playerStandWinDisplay = new();
            playerStandWinDisplay.PlayerTotal = _list[i].PlayerTotal;
            playerStandWinDisplay.DealerA = _list[i].Ev;
            playerStandWinDisplay.Dealer10 = _list[i + 7].Ev;
            playerStandWinDisplay.Dealer9 = _list[i + 14].Ev;
            playerStandWinDisplay.Dealer8 = _list[i + 21].Ev;
            playerStandWinDisplay.Dealer7 = _list[i + 28].Ev;
            playerStandWinDisplay.Dealer6 = _list[i + 35].Ev;
            playerStandWinDisplay.Dealer5 = _list[i + 42].Ev;
            playerStandWinDisplay.Dealer4 = _list[i + 49].Ev;
            playerStandWinDisplay.Dealer3 = _list[i + 56].Ev;
            playerStandWinDisplay.Dealer2 = _list[i + 63].Ev;

            displayList.Add(playerStandWinDisplay);
        }

        using (var writer = new StreamWriter($"data\\PlayerStandEv.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerStandEvDisplayMap>();
            csv.WriteRecords(displayList);
        }
    }
}