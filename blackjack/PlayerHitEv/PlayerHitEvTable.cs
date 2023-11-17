using static blackjack.HandType;
using static blackjack.Hand;
using static blackjack.Ev;

namespace blackjack;

public class PlayerHitEvTable
{
    private readonly PlayerStandEvTable _playerStandEvTable;
    private readonly Dictionary<(Hand, Hand), Ev> _dictionary = new();

    public PlayerHitEvTable(PlayerStandEvTable playerStandEvTable)
    {
        _playerStandEvTable = playerStandEvTable;
    }

    public void Compute()
    {
        _dictionary.Add((new(HARD, 21), Ace), Ev.Loss);
    }

    public Ev ComputeHard(Hand playerHand, Hand dealerHand)
    {
        if (playerHand.Total >= 22)
        {
            return Loss;
        }

        if (_dictionary.ContainsKey((playerHand, dealerHand)))
        {
            return _dictionary[(playerHand, dealerHand)];
        }

        return Win;
    }

    public void WriteToCsv()
    {
        Console.WriteLine(_dictionary.Count);
        Console.WriteLine(_dictionary.ToList().First());
    }
}