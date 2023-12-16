namespace blackjack;

public class Shoe 
{
    private readonly Stack<Card> _cards;
    private readonly int _numDecks;

    public Shoe(int numDecks)
    {
        _numDecks = numDecks;
        Card[] array = new Card[CardCount()];
        for (int i = 0; i < array.Length; i++)
        {
            int iMod13 = i % 13 + 1;
            int value = iMod13 >= 10 ? 10 : iMod13;
            array[i] = new(value);
        }

        Random random = new Random();
        random.Shuffle(array);

        _cards = new Stack<Card>(array);
    }

    public Card Pop()
    {
        Card card = _cards.Pop();
        return card;
    }

    public Hand Deal()
    {
        Hand hand = new Hand();
        hand = hand.Hit(Pop());
        hand = hand.Hit(Pop());
        return hand;
    }

    private int CardCount()
    {
        return _numDecks * 13 * 4;
    }

    public bool IsLive(int penetration)
    {
        int cardCount = CardCount();
        int currentCardCount = _cards.Count;
        int cardCountOutOfShoe = cardCount - currentCardCount;
        int percentage = (int)Math.Round((double)(cardCountOutOfShoe * 100) / cardCount);
        return percentage < penetration;
    }
}