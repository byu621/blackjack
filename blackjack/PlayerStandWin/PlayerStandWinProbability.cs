using CsvHelper.Configuration;

namespace blackjack;
public record PlayerStandWinProbability
{
    public int PlayerTotal { get; }
    public Probability DealerAce { get; }

    public PlayerStandWinProbability(int playerTotal, Probability dealerAce)
    {
        PlayerTotal = playerTotal;
        DealerAce = dealerAce;
    }
}

public sealed class PlayerStandWinProbabilityMap : ClassMap<PlayerStandWinProbability>
{
    public PlayerStandWinProbabilityMap()
    {
        Map(m => m.PlayerTotal).Name("#");
        Map(m => m.DealerAce).Name("A");
    }
}