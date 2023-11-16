using CsvHelper.Configuration;

namespace blackjack;
public struct PlayerStandWinProbability
{
    public int PlayerTotal { get; }
    public Hand DealerHand {  get; }
    public Probability Win { get; }

    public PlayerStandWinProbability(int playerTotal, Hand dealerHand, Probability win)
    {
        PlayerTotal = playerTotal;
        DealerHand = dealerHand;
        Win = win;
    }
}