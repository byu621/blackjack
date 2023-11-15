using CsvHelper;
using System.Globalization;
using static blackjack.DealerTotalProbability;
using static blackjack.Probability;
using static blackjack.HandType;

namespace blackjack;

/**
 * Each hard hand total 4..21 -> P(17), P(18), P(19), P(20), P(21), P(B)
 */
public class DealerTotalTable
{
    private readonly Dictionary<Hand, DealerTotalProbability> _dictionary = new();

    public void Compute()
    {
        _dictionary.Add(new Hand(HARD, 21), new(new Hand(HARD, 21), Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(new Hand(HARD, 20), new(new Hand(HARD, 20), Zero, Zero, Zero, One, Zero, Zero));
        _dictionary.Add(new Hand(HARD, 19), new(new Hand(HARD, 19), Zero, Zero, One, Zero, Zero, Zero));
        _dictionary.Add(new Hand(HARD, 18), new(new Hand(HARD, 18), Zero, One, Zero, Zero, Zero, Zero));
        _dictionary.Add(new Hand(HARD, 17), new(new Hand(HARD, 17), One, Zero, Zero, Zero, Zero, Zero));

        _dictionary.Add(new Hand(SOFT, 21), new(new Hand(SOFT, 21), Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(new Hand(SOFT, 20), new(new Hand(SOFT, 20), Zero, Zero, Zero, One, Zero, Zero));
        _dictionary.Add(new Hand(SOFT, 19), new(new Hand(SOFT, 19), Zero, Zero, One, Zero, Zero, Zero));
        _dictionary.Add(new Hand(SOFT, 18), new(new Hand(SOFT, 18), Zero, One, Zero, Zero, Zero, Zero));
        _dictionary.Add(new Hand(SOFT, 17), new(new Hand(SOFT, 17), One, Zero, Zero, Zero, Zero, Zero));

        for (int total = 16; total >= 2; total--)
        {
            _dictionary.Add(new Hand(HARD, total), ComputeHard(total));
        }
    }

    private DealerTotalProbability ComputeHard(int total)
    {
        if (total <= 1)
        {
            throw new ArgumentException($"total is invalid total = {total}");
        }

        if (total >= 22)
        {
            return HARD_22;
        }

        if (_dictionary.ContainsKey(new Hand(HARD, total)))
        {
            return _dictionary[new Hand(HARD, total)];
        }

        Probability p17 = Zero;
        Probability p18 = Zero;
        Probability p19 = Zero;
        Probability p20 = Zero;
        Probability p21 = Zero;
        Probability pBust = Zero;
        DealerTotalProbability dealerTotalProbability;
        for (int hitCard = 1; hitCard <= 10; hitCard++)
        {
            Probability probability = hitCard == 10 ? FourThirteenth : OneThirteenth;
            dealerTotalProbability = hitCard == 1 ? ComputeSoft(total + 11) : ComputeHard(total + hitCard);

            p17 += dealerTotalProbability.P17 * probability;
            p18 += dealerTotalProbability.P18 * probability;
            p19 += dealerTotalProbability.P19 * probability;
            p20 += dealerTotalProbability.P20 * probability;
            p21 += dealerTotalProbability.P21 * probability;
            pBust += dealerTotalProbability.PBust * probability;
        }

        return new(new Hand(HARD, total), p17, p18, p19, p20, p21, pBust);
    }

    private DealerTotalProbability ComputeSoft(int total)
    {
        if (total <= 0)
        {
            throw new ArgumentOutOfRangeException($"total is invalid total = {total}");
        }

        if (total >= 22)
        {
            return ComputeHard(total - 10);
        }

        if (_dictionary.ContainsKey(new Hand(SOFT, total)))
        {
            return _dictionary[new Hand(SOFT, total)];
        }

        Probability p17 = Zero;
        Probability p18 = Zero;
        Probability p19 = Zero;
        Probability p20 = Zero;
        Probability p21 = Zero;
        Probability pBust = Zero;
        DealerTotalProbability dealerTotalProbability;
        for (int hitCard = 1; hitCard <= 10; hitCard++)
        {
            Probability probability = hitCard == 10 ? FourThirteenth : OneThirteenth;
            dealerTotalProbability = ComputeSoft(total + hitCard);

            p17 += dealerTotalProbability.P17 * probability;
            p18 += dealerTotalProbability.P18 * probability;
            p19 += dealerTotalProbability.P19 * probability;
            p20 += dealerTotalProbability.P20 * probability;
            p21 += dealerTotalProbability.P21 * probability;
            pBust += dealerTotalProbability.PBust * probability;
        }

        _dictionary.Add(new Hand(SOFT, total), new(new Hand(SOFT, total), p17, p18, p19, p20, p21, pBust));
        return _dictionary[new Hand(SOFT, total)];
    }

    public void WriteToCsv()
    {
        using (var writer = new StreamWriter($"data\\DealerTotal.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<DealerTotalProbabilityMap>();

            csv.WriteRecords(_dictionary.Values);
        }
    }
}