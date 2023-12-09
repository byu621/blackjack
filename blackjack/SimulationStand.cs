namespace blackjack;

public class SimulationStand(int numDecksInShoe, int penetration)
{
    private readonly StandTable _standTable = new();

    public (decimal, StandTable) SimulateStand(int numShoe)
    {
        decimal ev = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateStand();
        }

        return (ev/numShoe, _standTable);
    }

    private decimal SimulateStand()
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        decimal runningEv = 0;
        int count = 0;
        while (shoe.isLive(penetration))
        {
            count++;
            Hand dealer = shoe.Deal();
            Card dealerUpCard = dealer.FirstCard!;
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
            _standTable.Add(dealerUpCard, player, ev);
        }

        return runningEv/count;
    }
}