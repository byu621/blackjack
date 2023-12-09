namespace blackjack.unittest;

public class ShoeTest
{
    [Fact]
    public void TestShoeDeal()
    {
        Shoe shoe = new Shoe(6);
        Hand hand = shoe.Deal();
        Assert.NotNull(hand.FirstCard);
    }
}