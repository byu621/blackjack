using CsvHelper.Configuration;
using static blackjack.Probability;

using static blackjack.HandType;

namespace blackjack;
public record DealerTotalProbability
{
    public Hand Hand { get; }
    public Probability P17 { get; }
    public Probability P18 { get; }
    public Probability P19 { get; }
    public Probability P20 { get; }
    public Probability P21 { get; }
    public Probability PBust { get; }

    public DealerTotalProbability(Hand hand, Probability p17,
        Probability p18,
        Probability p19,
        Probability p20,
        Probability p21,
        Probability pBust)
    {
        if (p17.IsNegative() || p18.IsNegative() || p19.IsNegative() || p20.IsNegative() || p21.IsNegative() || pBust.IsNegative())
        {
            throw new ArgumentException($"Probabilities can't be negative P17={p17} P18={p18} P19={p19} P20={p20} P21={p21} PBust={pBust}");
        }

        Hand = hand;
        P17 = p17;
        P18 = p18;
        P19 = p19;
        P20 = p20;
        P21 = p21;
        PBust = pBust;
    }

    public static DealerTotalProbability HARD_22 = new DealerTotalProbability(new Hand(HARD, 22), Zero, Zero, Zero, Zero, Zero, One);
}

public sealed class DealerTotalProbabilityMap : ClassMap<DealerTotalProbability>
{
    public DealerTotalProbabilityMap()
    {
        Map(m => m.Hand.HandType).Name("Type");
        Map(m => m.Hand.Total).Name("#");
        Map(m => m.P17);
        Map(m => m.P18);
        Map(m => m.P19);
        Map(m => m.P20);
        Map(m => m.P21);
        Map(m => m.PBust);
    }
}