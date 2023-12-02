namespace blackjack;

public record Card
{
    private readonly int value;

    public Card(int value )
    {
        this.value = value;
    }
}

