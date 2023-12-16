namespace blackjack;

public record Hand
{
    public int Value => _cards.Sum(card => card.Value);
    public Shape Shape => Shape.Hard;
    public bool Blackjack => _cards.Count == 2 && Value == 21;
    public bool IsBust => Value > 21;

    private readonly List<Card> _cards;

    public Hand()
    {
        _cards = new();
    }

    private Hand(Hand hand, Card card)
    {
        List<Card> cards = new(hand._cards) { card };
        _cards = cards;
    }

    public Hand Hit(Card card)
    {
        return new(this, card);
    }
}