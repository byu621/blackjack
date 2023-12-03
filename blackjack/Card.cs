namespace blackjack;

public record Card
{
    public int Value {get;}

    public Card(int value)
    {
        Value = value;
    }
}

