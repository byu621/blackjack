namespace blackjack;

public class SimulationHit(int numDecksInShoe, int penetration, StandTable standTable)
{
    private readonly HitTable _hitTable = new();

    public (decimal, HitTable) SimulateHit(int numShoe)
    {
        decimal ev = 0;
        for (int value = 20; value >= 19; value--)
        {
            for (int i = 0; i < numShoe; i++)
            {
                ev += SimulateHit(value, Shape.HARD);
            }
        }

        return (ev/numShoe/2, _hitTable);
    }

    private decimal SimulateHit(int value, Shape shape)
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        decimal runningEv = 0;
        int count = 0;
        while (shoe.isLive(penetration))
        {
            Hand dealer = shoe.Deal();
            Card dealerUpCard = dealer.UpCard!;
            Hand player = shoe.Deal();

            if (player.Value != value || player.Shape != shape) continue;
            count++;

            (bool isBlackjack, decimal ev) = player.EvaluateBlackjack(dealer);
            if (isBlackjack)
            {
                runningEv += ev;
                continue;
            }

            (Hand playerHand, bool playerBust) = player.Hit(shoe.Pop());
            if (playerBust)
            {
                ev = -1;
                runningEv += ev;
                _hitTable.Add(dealerUpCard, player, ev);
                continue;
            }
            
            if (playerHand.Value == 21)
            {
                (dealer, bool dealerBust) = shoe.DealerHit(dealer);
                ev = player.EvaluateHand(dealer, dealerBust);
                runningEv += ev;
                _hitTable.Add(dealerUpCard, player, ev);
                continue;
            }

            decimal standEv = standTable.Get(dealerUpCard, playerHand);
            decimal hitEv = _hitTable.Get(dealerUpCard, playerHand);
            if (hitEv == 0) throw new Exception("What");

            decimal maxEv = Math.Max(standEv, hitEv);
            runningEv += maxEv;
            _hitTable.Add(dealerUpCard, player, ev);
        }

        return count == 0 ? runningEv : runningEv/count;
    }
}