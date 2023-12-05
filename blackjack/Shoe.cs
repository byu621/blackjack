namespace blackjack;

public class Shoe 
{
    private readonly Stack<Card> cards = new();
    private readonly int numDecks;

    public Shoe(int numDecks)
    {
        Card[] array = new Card[CardCount(numDecks)];
        for (int i = 0; i < array.Length; i++)
        {
            int iMod13 = i % 13 + 1;
            int value = iMod13 >= 10 ? 10 : iMod13;
            array[i] = new(value);
        }

        Random random = new Random();
        random.Shuffle(array);

        cards = new Stack<Card>(array);
        this.numDecks = numDecks;
    }

    public Card Pop()
    {
        Card card = cards.Pop();
        return card;
    }

    public Hand Deal()
    {
        Hand hand = new Hand();
        (hand, _) = hand.Hit(Pop());
        (hand, _) = hand.Hit(Pop());
        return hand;
    }

    public (Hand, bool) DealerHit(Hand dealer)
    {
        bool dealerBust = false;
        while (!dealerBust && dealer.Value < 17)
        {
            (dealer, dealerBust) = dealer.Hit(Pop());
        }

        return (dealer, dealerBust);
    }

    private int CardCount(int numDecks)
    {
        return numDecks * 13 * 4;
    }

    public bool isLive(int penetration)
    {
        int cardCount = CardCount(numDecks);
        int currentCardCount = cards.Count;
        int cardCountOutOfShoe = cardCount - currentCardCount;
        int percentage = (int)Math.Round((double)(cardCountOutOfShoe * 100) / cardCount);
        return percentage < penetration;
    }
}