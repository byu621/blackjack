namespace blackjack.Structs;

public record Probability(decimal Value)
{
    public bool IsNegative()
    {
        return Value < 0;
    }

    public static Probability operator *(Probability a, Probability b)
    {
        return new(a.Value * b.Value);
    }
    public static Probability operator +(Probability a, Probability b)
    {
        return new(a.Value + b.Value);
    }

    public static readonly Probability Zero = new(0);
    public static readonly Probability One = new(1);
    public static readonly Probability OneThirteenth = new((decimal)1 / 13);
    public static readonly Probability FourThirteenth = new((decimal)4 / 13);

    public override string ToString()
    {
        decimal percentage = Value * 100;
        return $"{percentage.ToString("0.00")}%";
    }
}

