namespace blackjack;

public class Shoe 
{
    private readonly List<Card> cards = new();

    public Shoe(int numDecks)
    {
        for (int i = 0; i < numDecks; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 13; k++)
                {
                    cards.Add(new Card(k + 1));
                }
            }
        }

        System.Console.WriteLine(cards.Count);
    }
}