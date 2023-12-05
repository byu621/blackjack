namespace blackjack;

public class SimulationHit
{
    private readonly int numDecksInShoe;
    private readonly int penetration;
    private readonly StandTable standTable;

    public SimulationHit(int numDecksInShoe, int penetration, StandTable standTable)
    {
        this.numDecksInShoe = numDecksInShoe;
        this.penetration = penetration;
        this.standTable = standTable;
    }

    public (decimal, StandTable) SimulateHit(int numShoe)
    {
        decimal ev = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateHit();
        }

        return (ev/numShoe, standTable);
    }

    private decimal SimulateHit()
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        decimal runningEv = 0;
        int count = 0;
        while (shoe.isLive(penetration))
        {
            count++;
            Hand dealer = shoe.Deal();
            Hand player = shoe.Deal();

            (bool isBlackjack, decimal ev) = player.EvaluateBlackjack(dealer);
            if (isBlackjack)
            {
                runningEv += ev;
                continue;
            }

            (dealer, bool dealerBust) = shoe.DealerHit(dealer);

            ev = player.EvaluateHand(dealer, dealerBust);
            runningEv += ev;
        }

        return runningEv/count;
    }
}