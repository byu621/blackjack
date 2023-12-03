namespace blackjack;

public class Hand
{
    private readonly int value;
    private readonly Shape shape;
    private readonly bool soloTen;
    private readonly bool soloAce;
    public bool Blackjack { get; }

    public Hand()    
    {
    }

    private Hand(int value, Shape shape, bool soloTen, bool soloAce, bool blackjack)
    {
        if (value > 21) throw new ArgumentException();
        if (soloTen && value != 10) throw new ArgumentException();
        if (soloAce && value != 11) throw new ArgumentException($"{value},{shape},{soloTen},{soloAce},{blackjack}");
        if (blackjack && value != 21) throw new ArgumentException();

        this.value = value;
        this.shape = shape;
        this.soloTen = soloTen;
        this.soloAce = soloAce;
        Blackjack = blackjack;
    }

    public (Hand, bool) Hit(Card card)
    {
        int cardValue = card.Value == 1 && this.shape == Shape.HARD && this.value <= 10 ? 11 : card.Value;
        int value = this.value + card.Value;
        Shape shape = this.shape == Shape.SOFT || cardValue == 11 ? Shape.SOFT : Shape.HARD;
        bool soloTen = this.value == 0 && card.Value == 10;
        bool soloAce = this.value == 0 && card.Value == 1;
        bool blackjack = this.soloTen && card.Value == 1 || this.soloAce && card.Value == 10;

        if (value > 21 && shape == Shape.SOFT)
        {
            value -= 10;
            shape = Shape.HARD;
        }

        if (value > 21 && shape == Shape.HARD)
        {
            return (new(), true);
        }

        return (new (value, shape, soloTen, soloAce, blackjack), false);
    }
}