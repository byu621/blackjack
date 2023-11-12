using static blackjack.Structs.Probability;

namespace blackjack.Structs;
public record DealerStayTotalProbability
{
    public Probability P17 { get; }
    public Probability P18 { get; }
    public Probability P19 { get; }
    public Probability P20 { get; }
    public Probability P21 { get; }
    public Probability PBust { get; }

    public DealerStayTotalProbability(Probability p17,
        Probability p18,
        Probability p19,
        Probability p20,
        Probability p21,
        Probability pBust)
    {
        Probability total = p17 + p18 + p19 + p20 + p21 + pBust;
        if ((total) != One)
        {
            throw new ArgumentException($"Probabilities don't add up to one P17={p17} P18={p18} P19={p19} P20={p20} P21={p21} PBust={pBust} Total={total}");
        }

        P17 = p17; 
        P18 = p18;
        P19 = p19; 
        P20 = p20; 
        P21 = p21;
        PBust = pBust;
    }

    public static DealerStayTotalProbability OVER_21 = new DealerStayTotalProbability(Zero, Zero, Zero, Zero, Zero, One);
}