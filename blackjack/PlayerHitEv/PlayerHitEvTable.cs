using static blackjack.HandType;
using static blackjack.Hand;
using static blackjack.Ev;
using static blackjack.Probability;
using CsvHelper;
using System.Globalization;

namespace blackjack;

public class PlayerHitEvTable
{
    private readonly PlayerStandEvTable _playerStandEvTable;
    private readonly Dictionary<(Hand, Hand), Ev> _dictionary = new();
    private readonly Hand[] _dealerHands =  {
        new Hand(SOFT, 11), new Hand(HARD, 10), new Hand(HARD, 9), new Hand(HARD, 8), new Hand(HARD, 7), new Hand(HARD, 6), new Hand(HARD, 5),  new Hand(HARD, 4),  new Hand(HARD, 3), new Hand(HARD, 2)
    };

    private readonly Hand[] _playerHands = {
        new (BLACKJACK,21), new (HARD, 21), new  (HARD, 20), new (HARD, 19), new(HARD,18), new(HARD,17), new(HARD,16), new (HARD, 15), new (HARD, 14), new (HARD, 13), new (HARD, 12), new (HARD, 11), new (HARD, 10), new (HARD, 9), new (HARD, 8), new (HARD, 7), new (HARD, 6), new (HARD, 5), new (HARD, 4), new(SOFT,21)
    };

    public PlayerHitEvTable(PlayerStandEvTable playerStandEvTable)
    {
        _playerStandEvTable = playerStandEvTable;
    }

    public void Compute()
    {
        foreach(Hand hand in _playerHands)
        {
            foreach(Hand dealerHand in _dealerHands)
            {
                if (hand.HandType == BLACKJACK)
                {
                    ComputeBlackjack(dealerHand);
                }

                if (hand.HandType == SOFT)
                {
                    ComputeSoft(hand.Total, dealerHand);
                }
                else
                {
                    ComputeHard(hand.Total, dealerHand);
                }
            }
        }
    }

    public Ev ComputeHard(int playerTotal, Hand dealerHand)
    {
        if (playerTotal >= 22)
        {
            return Loss;
        }

        Hand playerHand = new Hand(HARD, playerTotal);

        if (_dictionary.ContainsKey((playerHand, dealerHand)))
        {
            return _dictionary[(playerHand, dealerHand)];
        }

        Ev stand = _playerStandEvTable.Get(playerHand, dealerHand);
        Ev hit = Push;
        for (int hitCard = 1; hitCard <= 10; hitCard++)
        {
            Probability probability = hitCard == 10 ? FourThirteenth : OneThirteenth;

            if (hitCard == 1)
            {
                hit += ComputeSoft(playerTotal + 11, dealerHand) * probability;
            }

            hit += ComputeHard(playerTotal + hitCard, dealerHand) * probability;
        }

        Ev maxEv = Max(hit, stand);
        _dictionary.Add((playerHand, dealerHand), maxEv);
        return maxEv;
    }

    public Ev ComputeSoft(int playerTotal, Hand dealerHand)
    {
        if (playerTotal >= 22)
        {
            return ComputeHard(playerTotal - 10, dealerHand);
        }

        Hand playerHand = new Hand(SOFT, playerTotal);

        if (_dictionary.ContainsKey((playerHand, dealerHand)))
        {
            return _dictionary[(playerHand, dealerHand)];
        }

        Ev stand = _playerStandEvTable.Get(playerHand, dealerHand);
        Ev hit = Push;
        for (int hitCard = 1; hitCard <= 10; hitCard++)
        {
            Probability probability = hitCard == 10 ? FourThirteenth : OneThirteenth;

            hit += ComputeSoft(playerTotal + hitCard, dealerHand) * probability;
        }

        Ev maxEv = Max(hit, stand);
        _dictionary.Add((playerHand, dealerHand), maxEv);
        return maxEv;
    }

    public Ev ComputeBlackjack(Hand dealerHand)
    {
        Ev ev = _playerStandEvTable.Get(Hand.Blackjack, dealerHand);
        _dictionary.Add((Hand.Blackjack, dealerHand), ev);
        return ev;
    }

    public void WriteToCsv()
    {
        List<PlayerHitEvDisplay> displayList = new();

        foreach (Hand playerHand in _playerHands)
        {
            PlayerHitEvDisplay playerHitEvDisplay = new();
            playerHitEvDisplay.PlayerHand = playerHand;

            Ev dealerA = _dictionary[(playerHand, _dealerHands[0])];
            playerHitEvDisplay.DealerA = dealerA;

            Ev dealer10 = _dictionary[(playerHand, _dealerHands[1])];
            playerHitEvDisplay.Dealer10 = dealer10;

            Ev dealer9 = _dictionary[(playerHand, _dealerHands[2])];
            playerHitEvDisplay.Dealer9 = dealer9;

            Ev dealer8 = _dictionary[(playerHand, _dealerHands[3])];
            playerHitEvDisplay.Dealer8 = dealer8;

            Ev dealer7 = _dictionary[(playerHand, _dealerHands[4])];
            playerHitEvDisplay.Dealer7 = dealer7;

            Ev dealer6 = _dictionary[(playerHand, _dealerHands[5])];
            playerHitEvDisplay.Dealer6 = dealer6;

            Ev dealer5 = _dictionary[(playerHand, _dealerHands[6])];
            playerHitEvDisplay.Dealer5 = dealer5;

            Ev dealer4 = _dictionary[(playerHand, _dealerHands[7])];
            playerHitEvDisplay.Dealer4 = dealer4;

            Ev dealer3 = _dictionary[(playerHand, _dealerHands[8])];
            playerHitEvDisplay.Dealer3 = dealer3;

            Ev dealer2 = _dictionary[(playerHand, _dealerHands[9])];
            playerHitEvDisplay.Dealer2 = dealer2;

            displayList.Add(playerHitEvDisplay);
        }

        using (var writer = new StreamWriter($"data\\PlayerHitEv.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<PlayerHitEvDisplayMap>();
            csv.WriteRecords(displayList);
        }
    }
}