namespace blackjack;

public class Simulation
{
    private readonly int numDecksInShoe;
    private readonly int penetration;
    private readonly StandTable standTable;

    public Simulation(int numDecksInShoe, int penetration)
    {
        this.numDecksInShoe = numDecksInShoe;
        this.penetration = penetration;
        standTable = new();
    }

    public decimal SimulateStand(int numShoe)
    {
        decimal ev = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateStand();
        }

        Console.WriteLine(standTable.ToString());
        return ev/numShoe;
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
            Card dealerUpCard = dealer.UpCard!;
            Hand player = shoe.Deal();

            (bool isBlackjack, decimal ev) = player.EvaluateBlackjack(dealer);
            if (isBlackjack)
            {
                runningEv += ev;
                continue;
            }

            bool dealerBust = false;
            while (!dealerBust && dealer.Value < 17)
            {
                (dealer, dealerBust) = dealer.Hit(shoe.Pop());
            }

            ev = player.EvaluateHand(dealer, dealerBust);
            runningEv += ev;
            standTable.Add(dealerUpCard, player, ev);
        }

        return runningEv/count;
    }
}