namespace blackjack;

public struct State(Shape shape, int value, int dealer)
{
    public Shape Shape = shape;
    public int Value = value;
    public int Dealer = dealer;
}