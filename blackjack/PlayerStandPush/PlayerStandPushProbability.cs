using CsvHelper.Configuration;

namespace blackjack;
public struct PlayerStandPushProbability
{
    public int PlayerTotal { get; }
    public Hand DealerHand {  get; }
    public Probability Push { get; }

    public PlayerStandPushProbability(int playerTotal, Hand dealerHand, Probability push)
    {
        PlayerTotal = playerTotal;
        DealerHand = dealerHand;
        Push = push;
    }
}