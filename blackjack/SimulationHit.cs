namespace blackjack;

public class SimulationHit(int numDecksInShoe, int penetration)
{
    private readonly HitTable _hitTable = new();

    public (decimal, HitTable) SimulateHit(int numShoe)
    {
        decimal ev = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateHit();
        }

        return (ev/numShoe, _hitTable);
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
            Card dealerUpCard = dealer.UpCard!;
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
            _hitTable.Add(dealerUpCard, player, ev);
        }

        return runningEv/count;
    }
}