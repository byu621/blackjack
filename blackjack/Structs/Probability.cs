﻿namespace blackjack;

public record Probability
{
    public decimal Value { get; }

    public Probability(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException($"Probability can't be negative probability = {value}");
        }

        Value = value;
    }

    public static Probability operator *(Probability a, Probability b)
    {
        return new(a.Value * b.Value);
    }
    public static Probability operator +(Probability a, Probability b)
    {
        return new(a.Value + b.Value);
    }

    public static Probability operator -(Probability a, Probability b)
    {
        return new(a.Value - b.Value);
    }

    public static readonly Probability Zero = new(0);
    public static readonly Probability One = new(1);
    public static readonly Probability OneThirteenth = new((decimal)1 / 13);
    public static readonly Probability FourThirteenth = new((decimal)4 / 13);
    public static readonly Probability Blackjack = new((decimal)1 / 4 * 1 / 13 * 2);
    public static readonly Probability BlackjackGivenAce = new((decimal)4 / 13);
    public static readonly Probability BlackjackGivenTen = new((decimal)1 / 13);

    public override string ToString()
    {
        decimal percentage = Value * 100;
        return $"{percentage.ToString("0.00")}%";
    }
}