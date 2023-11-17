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

    public static Ev CalcBlackjack(Probability win, Probability lose)
    {
        decimal value = (win.Value - lose.Value) * Blackjack3To2;
        return new Ev(value);
    }

    private static decimal Blackjack3To2 = (decimal)1.5;

    public override string ToString()
    {
        decimal percentage = Value * 100;
        return $"{percentage.ToString("0.00")}%";
    }
}