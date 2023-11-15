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

        // possible increases to a hand's total is 1..10
        for (int startingHand = 16; startingHand >= 2; startingHand--)
        {
            Probability p17 = Zero;
            Probability p18 = Zero;
            Probability p19 = Zero;
            Probability p20 = Zero;
            Probability p21 = Zero;
            Probability pBust = Zero;
            for (int hitCard = 1; hitCard <= 10; hitCard++)
            {
                int handTotal = startingHand + hitCard;
                Probability handTotalProbability = hitCard == 10 ? FourThirteenth : OneThirteenth;

                p17 += _dictionary.GetValueOrDefault(new Hand(HARD, handTotal), HARD_22).P17 * handTotalProbability;
                p18 += _dictionary.GetValueOrDefault(new Hand(HARD, handTotal), HARD_22).P18 * handTotalProbability;
                p19 += _dictionary.GetValueOrDefault(new Hand(HARD, handTotal), HARD_22).P19 * handTotalProbability;
                p20 += _dictionary.GetValueOrDefault(new Hand(HARD, handTotal), HARD_22).P20 * handTotalProbability;
                p21 += _dictionary.GetValueOrDefault(new Hand(HARD, handTotal), HARD_22).P21 * handTotalProbability;
                pBust += _dictionary.GetValueOrDefault(new Hand(HARD, handTotal), HARD_22).PBust * handTotalProbability;
            }

            _dictionary.Add(new Hand(HARD, startingHand), new(new Hand(HARD, startingHand), p17, p18, p19, p20, p21, pBust));
        }
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