namespace blackjack.unittest;

public class HandTest
{
    [Fact]
    public void TestHandConstructor()
    {
        Hand hand = new();
        Assert.Equal(0, hand.Value);
        Assert.Equal(Shape.HARD, hand.Shape);
    }

    [Fact]
    public void TestHitAce()
    {
        Hand hand = new Hand();
        (hand,_) = hand.Hit(new (1));

        Assert.Equal(11, hand.Value);
        Assert.True(hand.SoloAce);
    }

    [Fact]
    public void TestHitTen()
    {
        Hand hand = new Hand();
        (hand,_) = hand.Hit(new (10));

        Assert.Equal(10, hand.Value);
        Assert.True(hand.SoloTen);
    }

    [Fact]
    public void TestHitBlackjack()
    {
        Hand hand = new Hand();
        (hand,_) = hand.Hit(new (10));
        (hand,_) = hand.Hit(new (1));

        Assert.Equal(21, hand.Value);
        Assert.Equal(Shape.SOFT, hand.Shape);
        Assert.True(hand.Blackjack);
    }
    
    [Fact]
    public void TestUpCard()
    {
        Hand hand = new Hand();
        (hand, _) = hand.Hit(new(1));
        Assert.Equal(1, hand.UpCard!.Value);
    }
}