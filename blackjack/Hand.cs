namespace blackjack;

public class Hand
{
    public int Value { get; }
    public Shape Shape { get; }
    public bool SoloTen { get; }
    public bool SoloAce {get;}
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

        Value = value;
        Shape = shape;
        this.SoloTen = soloTen;
        this.SoloAce = soloAce;
        Blackjack = blackjack;
    }

    public (Hand, bool) Hit(Card card)
    {
        int cardValue = card.Value == 1 && Shape == Shape.HARD && Value <= 10 ? 11 : card.Value;
        int value = Value + cardValue;
        Shape shape = Shape == Shape.SOFT || cardValue == 11 ? Shape.SOFT : Shape.HARD;
        bool soloTen = Value == 0 && card.Value == 10;
        bool soloAce = Value == 0 && card.Value == 1;
        bool blackjack = this.SoloTen && card.Value == 1 || this.SoloAce && card.Value == 10;

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