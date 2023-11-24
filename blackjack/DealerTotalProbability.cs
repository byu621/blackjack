namespace blackjack;

public record DealerTotalProbability
{
    private readonly decimal p17;
    private readonly decimal p18;
    private readonly decimal p19;
    private readonly decimal p20;
    private readonly decimal p21;
    private readonly decimal pBust;

    public DealerTotalProbability(decimal p17, decimal p18, decimal p19, decimal p20, decimal p21, decimal pBust)
    {
        this.p17 = p17;
        this.p18 = p18;
        this.p19 = p19;
        this.p20 = p20;
        this.p21 = p21;
        this.pBust = pBust;
    }

    public DealerTotalProbability()
    {

    }

    public DealerTotalProbability Add(DealerTotalProbability other, decimal probability)
    {
        decimal newP17 = p17 + other.p17 * probability;
        decimal newP18 = p18 + other.p18 * probability;
        decimal newP19 = p19 + other.p19 * probability;
        decimal newP20 = p20 + other.p20 * probability;
        decimal newP21 = p21 + other.p21 * probability;
        decimal newPBust = pBust + other.pBust * probability;
        return new DealerTotalProbability(newP17, newP18, newP19, newP20, newP21, newPBust);
    }

    public override string ToString()
    {
        return $"{p17 * 100:0.00}%,{p18 * 100:0.00}%,{p19 * 100:0.00}%,{p20 * 100:0.00}%,{p21 * 100:0.00}%,{pBust * 100:0.00}%";
    }
}