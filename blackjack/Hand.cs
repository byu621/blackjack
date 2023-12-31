namespace blackjack;

public record Hand
{
    private int HardValue => _cards.Sum(card => card.Value);
    private int SoftValue => HasAce ? HardValue + 10 : HardValue;
    public int Value => SoftValue <= 21 ? SoftValue : HardValue;
    public Shape Shape => HasAce && SoftValue <= 21 ? Shape.Soft : Shape.Hard;
    public bool Blackjack => _cards.Count == 2 && SoftValue == 21;
    public Card DealerUpCard => _cards[0];
    public bool IsPair => _cards.Count == 2 && _cards[0].Value == _cards[1].Value;
    public bool IsBust => HardValue > 21;
    private bool HasAce => _cards.Any(c => c.Value == 1);

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