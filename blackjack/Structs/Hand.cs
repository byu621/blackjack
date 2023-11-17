using static blackjack.HandType;

namespace blackjack;

public readonly struct Hand
{
    public HandType HandType { get; }
    public int Total { get; }

    public Hand(HandType handType, int total)
    {
        HandType = handType;
        Total = total;
    }

    public static Hand Blackjack = new Hand(BLACKJACK, 21);
    public static Hand Ace = new Hand(SOFT, 11);
    public static Hand Ten = new Hand(HARD, 10);
}