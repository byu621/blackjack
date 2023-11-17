namespace blackjack;

public record Ev
{
    public decimal Value { get; }

    public Ev(decimal value)
    {
        Value = value;
    }

    public static Ev Calc(Probability win, Probability lose)
    {
        return new Ev(win.Value - lose.Value);
    }

    public override string ToString()
    {
        decimal percentage = Value * 100;
        return $"{percentage.ToString("0.00")}%";
    }
}