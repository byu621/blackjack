using CsvHelper.Configuration;

namespace blackjack;
public record PlayerStandWinProbability
{
    public Hand PlayerHand { get; }
    public Probability DealerAce { get; }

    public PlayerStandWinProbability(Hand playerHand, Probability dealerAce)
    {
        PlayerHand = playerHand;
        DealerAce = dealerAce;
    }
}

public sealed class PlayerStandWinProbabilityMap : ClassMap<PlayerStandWinProbability>
{
    public PlayerStandWinProbabilityMap()
    {
        Map(m => m.PlayerHand.HandType).Name("Type");
        Map(m => m.PlayerHand.Total).Name("#");
        Map(m => m.DealerAce).Name("A");
    }
}