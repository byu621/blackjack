namespace blackjack;
public struct PlayerStandEvProbability
{
    public string PlayerTotal { get; }
    public Hand DealerHand {  get; }
    public Ev Ev { get; }

    public PlayerStandEvProbability(string playerTotal, Hand dealerHand, Ev ev)
    {
        PlayerTotal = playerTotal;
        DealerHand = dealerHand;
        Ev = ev;
    }
}