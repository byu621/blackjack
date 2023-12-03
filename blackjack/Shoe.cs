namespace blackjack;

public class Shoe 
{
    private readonly Stack<Card> cards = new();
    private readonly int numDecks;

    public Shoe(int numDecks)
    {
        for (int i = 0; i < CardCount(numDecks); i++)
        {
            int iMod13 = i % 13 + 1;
            int value = iMod13 >= 10 ? 10 : iMod13;
            cards.Push(new Card(value));
        }

        this.numDecks = numDecks;
    }

    public Card Pop()
    {
        Card card = cards.Pop();
        return card;
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