using CsvHelper.Configuration;

namespace blackjack;
public struct PlayerStandWinProbability
{
    public string PlayerTotal { get; }
    public Hand DealerHand {  get; }
    public Ev Ev { get; }

    public PlayerStandWinProbability(string playerTotal, Hand dealerHand, Ev ev)
    {
        PlayerTotal = playerTotal;
        DealerHand = dealerHand;
        Ev = ev;
    }
}