namespace blackjack.Structs;

public record Probability(int Numerator, int Denominator)
{
    public decimal ToDecimal()
    {
        return (decimal)Numerator / Denominator;
    }
    
    public static Probability operator *(Probability a, Probability b)
    {
        return new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static readonly Probability Zero = new(0, 1);
    public static readonly Probability One = new(1, 1);
    public static readonly Probability OneThirteenth = new(1, 13);
}

