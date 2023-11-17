using CsvHelper;
using System.Globalization;
using static blackjack.HandType;
using static blackjack.Probability;

namespace blackjack;

public class PlayerStandEvTable
{
    private readonly Dictionary<(Hand, Hand), Ev> _dictionary = new();
    private readonly DealerTotalTable _dealerTotalTable;
    private readonly Hand[] _dealerHands =  {
        new Hand(SOFT, 11), new Hand(HARD, 10), new Hand(HARD, 9), new Hand(HARD, 8), new Hand(HARD, 7), new Hand(HARD, 6), new Hand(HARD, 5),  new Hand(HARD, 4),  new Hand(HARD, 3), new Hand(HARD, 2)
    };
    private readonly Hand[] _playerHands =
    {
        Hand.Blackjack, new Hand(HARD, 21), new Hand (HARD, 20), new Hand(HARD, 19), new Hand(HARD, 18), new Hand(HARD, 17), new Hand(HARD, 16)
    };

    public PlayerStandEvTable(DealerTotalTable dealerTotalTable)
    {
        _dealerTotalTable = dealerTotalTable;
    }

    public void Compute()
    {
        foreach(Hand dealerHand in _dealerHands)
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
            ev = Ev.CalcBlackjack(win, lose);
            _dictionary.Add((Hand.Blackjack, dealerHand), ev);

            //21
            push = dealerProbability.P21 - dealerBlackjack;
            lose = dealerBlackjack;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _dictionary.Add((new Hand(HARD, 21), dealerHand), ev);

            //20
            push = dealerProbability.P20;
            lose = dealerProbability.P21;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _dictionary.Add((new Hand(HARD, 20), dealerHand), ev);

            //19
            push = dealerProbability.P19;
            lose = dealerProbability.P21 + dealerProbability.P20;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _dictionary.Add((new Hand(HARD, 19), dealerHand), ev);

            //18
            push = dealerProbability.P18;
            lose = dealerProbability.P21 + dealerProbability.P20 + dealerProbability.P19;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _dictionary.Add((new Hand(HARD, 18), dealerHand), ev);

            //17
            push = dealerProbability.P17;
            lose = dealerProbability.P21 + dealerProbability.P20 + dealerProbability.P19 + dealerProbability.P18;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _dictionary.Add((new Hand(HARD, 17), dealerHand), ev);

            //16
            push = Zero;
            lose = dealerProbability.P21 + dealerProbability.P20 + dealerProbability.P19 + dealerProbability.P18 + dealerProbability.P17;
            win = One - push - lose;
            ev = Ev.Calc(win, lose);
            _dictionary.Add((new Hand(HARD, 16), dealerHand), ev);
        }
    }

    public void WriteToCsv()
    {
        List<PlayerStandEvDisplay> displayList = new();

        foreach(Hand playerHand in _playerHands)
        {
            PlayerStandEvDisplay playerStandEvDisplay = new();
            playerStandEvDisplay.PlayerHand = playerHand;

            Ev dealerA = _dictionary[(playerHand, _dealerHands[0])];
            playerStandEvDisplay.DealerA = dealerA;

            Ev dealer10 = _dictionary[(playerHand, _dealerHands[1])];
            playerStandEvDisplay.Dealer10 = dealer10;

            Ev dealer9 = _dictionary[(playerHand, _dealerHands[2])];
            playerStandEvDisplay.Dealer9 = dealer9;

            Ev dealer8 = _dictionary[(playerHand, _dealerHands[3])];
            playerStandEvDisplay.Dealer8 = dealer8;

            Ev dealer7 = _dictionary[(playerHand, _dealerHands[4])];
            playerStandEvDisplay.Dealer7 = dealer7;

            Ev dealer6 = _dictionary[(playerHand, _dealerHands[5])];
            playerStandEvDisplay.Dealer6 = dealer6;

            Ev dealer5 = _dictionary[(playerHand, _dealerHands[6])];
            playerStandEvDisplay.Dealer5 = dealer5;

            Ev dealer4 = _dictionary[(playerHand, _dealerHands[7])];
            playerStandEvDisplay.Dealer4 = dealer4;

            Ev dealer3 = _dictionary[(playerHand, _dealerHands[8])];
            playerStandEvDisplay.Dealer3 = dealer3;

            Ev dealer2 = _dictionary[(playerHand, _dealerHands[9])];
            playerStandEvDisplay.Dealer2 = dealer2;

            displayList.Add(playerStandEvDisplay);
        }

        using (var writer = new StreamWriter($"data\\PlayerStandEv.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerStandEvDisplayMap>();
            csv.WriteRecords(displayList);
        }
    }
}