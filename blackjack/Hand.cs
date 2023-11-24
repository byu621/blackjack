namespace blackjack;

public record Hand 
{
    private readonly Shape shape;
    private readonly int value;
    private readonly bool soloTen;
    private readonly bool soloAce;
    private readonly bool blackjack;

    public Hand(Shape shape, int value, bool soloAce, bool soloTen, bool blackjack)
    {
        if (soloAce && (shape != Shape.SOFT || value != 11)) throw new ArgumentException("ERROR");
        if (soloTen && (shape != Shape.HARD || value != 10)) throw new ArgumentException("ERROR");
        if (blackjack && (shape != Shape.SOFT || value != 21)) throw new ArgumentException("ERROR");

        this.shape = shape;
        this.value = value;
        this.soloAce = soloAce;
        this.soloTen = soloTen;
        this.blackjack = blackjack;
    }

    public DealerTotalProbability CalculateDTP()
    {
        if (value > 21 && shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, value - 10, false, false, false);
            return hand.CalculateDTP();
        }
        
        if (value > 21) return new (0, 0, 0, 0, 0, 1);
        if (value == 21) return new (0, 0, 0, 0, 1, 0);
        if (value == 20) return new (0, 0, 0, 1, 0, 0);
        if (value == 19) return new (0, 0, 1, 0, 0, 0);
        if (value == 18) return new (0, 1, 0, 0, 0, 0);
        if (value == 17) return new (1, 0, 0, 0, 0, 0);

        DealerTotalProbability dealerTotalProbability = new();
        for (int hit = 1; hit <= 10; hit++)
        {
            decimal hitProbability = hit == 10 ? (decimal) 4 / 13 : (decimal) 1 / 13;
            Hand hand = Hit(hit);
            dealerTotalProbability = dealerTotalProbability.Add(hand.CalculateDTP(), hitProbability);
        }

        return dealerTotalProbability;
    }

    public decimal CalculatePSEV(Hand dealer)
    {
        DealerTotalProbability dtp = dealer.CalculateDTP();

        if (blackjack)
        {
            decimal push = dealer.ProbabilityBlackjack();
            decimal win = 1 - push;
            decimal lose = 0;
            return (win - lose) * 1.5m;
        }
        else if (value == 21)
        {
            decimal push = dtp.GetProbability(value) - dealer.ProbabilityBlackjack();
            decimal win = dtp.PBust + dtp.GetAccumulatedProbabilityBelowValue(value);
            decimal lose = 1 - push - win;
            return win - lose;
        }
        else 
        {
            decimal push = dtp.GetProbability(value);
            decimal win = dtp.PBust + dtp.GetAccumulatedProbabilityBelowValue(value);
            decimal lose = 1 - push - win;
            return win - lose;
        }
    }

    public decimal ProbabilityBlackjack()
    {
        if (blackjack) return 1;
        if (soloAce) return (decimal) 4 / 13;
        if (soloTen) return (decimal) 1 / 13;
        return 0;
    }

    public Hand Hit(int hit)
    {
        Shape newShape = shape == Shape.SOFT || hit == 1 ? Shape.SOFT : Shape.HARD;
        int newValue = hit == 1 && shape == Shape.HARD ? value + 11 : value + hit;
        bool isBlackjack = (soloAce && hit == 10) || (soloTen && hit == 1);
        return new Hand(newShape, newValue, false, false, isBlackjack);
    }

    public static List<Hand> GetDealerStartingHands()
    {
        List<Hand> list = new()
        {
            new Hand(Shape.SOFT, 11, true, false, false),
            new Hand(Shape.HARD, 10, false, true, false)
        };

        for (int i = 9; i >= 2; i--)
        {
            list.Add(new (Shape.HARD, i, false, false, false));
        }

        return list;
    }
}