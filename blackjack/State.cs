namespace blackjack;

public struct State(Shape shape, int value, int dealer)
{
    public Shape Shape = shape;
    public int Value = value;
    public int Dealer = dealer;
}

public struct PairState(Card card, int dealer)
{
    public Card Card = card;
    public int Dealer = dealer;
}