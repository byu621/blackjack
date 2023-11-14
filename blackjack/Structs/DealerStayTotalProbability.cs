﻿using CsvHelper.Configuration;
using static blackjack.Structs.Probability;

namespace blackjack.Structs;
public record DealerStayTotalProbability
{
    public int DealerTotal { get; }
    public Probability P17 { get; }
    public Probability P18 { get; }
    public Probability P19 { get; }
    public Probability P20 { get; }
    public Probability P21 { get; }
    public Probability PBust { get; }

    public DealerStayTotalProbability(int dealerTotal, Probability p17,
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

        DealerTotal = dealerTotal;
        P17 = p17;
        P18 = p18;
        P19 = p19;
        P20 = p20;
        P21 = p21;
        PBust = pBust;
    }

    public static DealerStayTotalProbability OVER_21 = new DealerStayTotalProbability(22, Zero, Zero, Zero, Zero, Zero, One);
}

public sealed class DealerStayTotalProbabilityMap : ClassMap<DealerStayTotalProbability>
{
    public DealerStayTotalProbabilityMap()
    {
        Map(m => m.DealerTotal);
        Map(m => m.P17).Name("P17");
        Map(m => m.P18).Name("P18");
        Map(m => m.P19).Name("P19");
        Map(m => m.P20).Name("P20");
        Map(m => m.P21).Name("P21");
        Map(m => m.PBust).Name("PBust");
    }
}