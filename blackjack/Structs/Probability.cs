namespace blackjack.Structs;

public record Probability(ulong Numerator, ulong Denominator)
{
    public static Probability operator *(Probability a, Probability b)
    {
        return new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }
    public static Probability operator +(Probability a, Probability b)
    {
        ulong commonDenominator = a.Denominator * b.Denominator;

        // Calculate the numerators for the common denominator
        ulong newNumerator1 = a.Numerator * b.Denominator;
        ulong newNumerator2 = b.Numerator * a.Denominator;

        // Add the numerators
        ulong sumOfNumerators = newNumerator1 + newNumerator2;

        // Simplify the result by finding the greatest common divisor (GCD)
        ulong gcd = FindGCD(sumOfNumerators, commonDenominator);

        // Divide both the numerator and denominator by the GCD
        ulong resultNumerator = sumOfNumerators / gcd;
        ulong resultDenominator = commonDenominator / gcd;

        return new(resultNumerator, resultDenominator);
    }

    public static readonly Probability Zero = new(0, 1);
    public static readonly Probability One = new(1, 1);
    public static readonly Probability OneThirteenth = new(1, 13);
    public static readonly Probability FourThirteenth = new(4, 13);

    public override string ToString()
    {
        //double result = (double)Numerator / Denominator * 100;
        //return $"{result.ToString("0.00")}%";
        return $"{Numerator}/{Denominator}";
    }

    private static ulong FindGCD(ulong a, ulong b)
    {
        while (b != 0)
        {
            ulong temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}

