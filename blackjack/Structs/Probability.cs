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
    public static Probability operator +(Probability a, Probability b)
    {
        int commonDenominator = a.Denominator * b.Denominator;

        // Calculate the numerators for the common denominator
        int newNumerator1 = a.Numerator * b.Denominator;
        int newNumerator2 = b.Numerator * a.Denominator;

        // Add the numerators
        int sumOfNumerators = newNumerator1 + newNumerator2;

        // Simplify the result by finding the greatest common divisor (GCD)
        int gcd = FindGCD(sumOfNumerators, commonDenominator);

        // Divide both the numerator and denominator by the GCD
        int resultNumerator = sumOfNumerators / gcd;
        int resultDenominator = commonDenominator / gcd;

        return new(resultNumerator, resultDenominator);
    }

    public static readonly Probability Zero = new(0, 1);
    public static readonly Probability One = new(1, 1);
    public static readonly Probability OneThirteenth = new(1, 13);
    public static readonly Probability FourThirteenth = new(4, 13);

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    private static int FindGCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}

