namespace blackjack;

public class SimulationHit(int numDecksInShoe, int penetration, StandTable standTable)
{
    private readonly HitTable _hitTable = new("hit");

    public (decimal, HitTable) SimulateHit(int numShoe)
    {
        decimal ev = 0;
        int count = 0;
        for (int value = 20; value >= 10; value--)
        {
            ev += SimulateNumShoe(numShoe, value, Shape.HARD);
            count++;
        }

        for (int value = 20; value >= 12; value--)
        {
            ev += SimulateNumShoe(numShoe, value, Shape.SOFT);
            count++;
        }
        
        for (int value = 9; value >= 4; value--)
        {
            ev += SimulateNumShoe(numShoe, value, Shape.HARD);
            count++;
        }

        return (ev/count, _hitTable);
    }
    
    private decimal SimulateNumShoe(int numShoe, int value, Shape shape)
    {
        decimal ev = 0;
        int count = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateHit(value, shape);
            count++;
        }

        return count == 0 ? ev : ev / count;
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

            (bool isBlackjack, decimal blackjackEv) = player.EvaluateBlackjack(dealer);
            if (isBlackjack)
            {
                runningEv += blackjackEv;
                continue;
            }

            (Hand playerHit, bool playerBust) = player.Hit(shoe.Pop());
            if (playerBust)
            {
                runningEv += -1m;
                _hitTable.Add(dealerUpCard, player, -1m);
                continue;
            }
            
            if (playerHit.Value == 21)
            {
                (dealer, bool dealerBust) = shoe.DealerHit(dealer);
                decimal ev = playerHit.EvaluateHand(dealer, dealerBust);
                runningEv += ev;
                _hitTable.Add(dealerUpCard, player, ev);
                continue;
            }

            decimal standEv = standTable.Get(dealerUpCard, playerHit);
            decimal hitEv = _hitTable.Get(dealerUpCard, playerHit);
            if (hitEv == 0) throw new Exception("HitEv is 0");
            decimal maxEv = Math.Max(standEv, hitEv);
            runningEv += maxEv;
            _hitTable.Add(dealerUpCard, player, maxEv);
        }

        return count == 0 ? runningEv : runningEv/count;
    }
}