namespace blackjack;

public record Hand 
{
    private readonly Shape shape;
    private readonly int value;
    private readonly bool soloTen;
    private readonly bool soloAce;
    private readonly bool blackjack;

    private static readonly Dictionary<(Hand, Hand), decimal> PlayerStandCache = new();

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

    public decimal CalculateDealerBust()
    {
        if (value > 21 && shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, value - 10, false, false, false);
            return hand.CalculateDealerBust();
        }

        if (value > 21) return 1;
        if (value >= 17) return 0;
        
        decimal probability = 0;
        for (int hit = 1; hit <= 10; hit++)
        {
            decimal hitProbability = hit == 10 ? (decimal) 4 / 13 : (decimal) 1 / 13;
            Hand hand = Hit(hit);
            probability += hand.CalculateDealerBust() * hitProbability;
        }

        return probability;
    }

    public decimal CalculateDTP(int target)
    {
        if (target < 17)
        {
            return 0;
        }

        if (value > 21 && shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, value - 10, false, false, false);
            return hand.CalculateDTP(target);
        }

        if (value > 21) return 0;
        if (value == target) return 1;
        if (value > target) return 0;
        if (value >= 17) return 0;
        
        decimal probability = 0;
        for (int hit = 1; hit <= 10; hit++)
        {
            decimal hitProbability = hit == 10 ? (decimal) 4 / 13 : (decimal) 1 / 13;
            Hand hand = Hit(hit);
            probability += hand.CalculateDTP(target) * hitProbability;
        }

        return probability;
    }

    private decimal CalculateDTPBelowTarget(int target)
    {
        decimal probability = 0;
        for (int i = 17; i < target; i++)
        {
            probability += CalculateDTP(i);
        }

        return probability;
    }

    public decimal CalculatePSEV(Hand dealer)
    {
        if (PlayerStandCache.ContainsKey((this, dealer)))
        {
            return PlayerStandCache[(this, dealer)];
        }

        decimal ev;
        if (blackjack)
        {
            decimal push = dealer.ProbabilityBlackjack();
            decimal win = 1 - push;
            decimal lose = 0;
            ev = (win - lose) * 1.5m;
        }
        else if (value == 21)
        {
            decimal push = dealer.CalculateDTP(value) - dealer.ProbabilityBlackjack();
            decimal win = dealer.CalculateDealerBust() + dealer.CalculateDTPBelowTarget(value);
            decimal lose = 1 - push - win;
            ev = win - lose;
        }
        else 
        {
            decimal push = dealer.CalculateDTP(value);
            decimal win = dealer.CalculateDealerBust() + dealer.CalculateDTPBelowTarget(value);
            decimal lose = 1 - push - win;
            ev = win - lose;
        }

        PlayerStandCache[(this, dealer)] = ev;
        return ev;
    }
    
    public decimal CalculatePHEV(Hand dealer)
    {
        if (value > 21 && shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, value - 10, false, false, false);
            return hand.CalculatePHEV(dealer);
        }

        if (value > 21)
        {
            return -1;
        }
        
        decimal standEv = CalculatePSEV(dealer);
        decimal ev = 0;
        for (int hit = 1; hit <= 10; hit++)
        {
            decimal hitProbability = hit == 10 ? (decimal) 4 / 13 : (decimal) 1 / 13;
            Hand hand = Hit(hit);
            ev += hand.CalculatePHEV(dealer) * hitProbability;
        }

        decimal hitEv = ev;
        return Math.Max(standEv, hitEv);
    }

    private decimal ProbabilityBlackjack()
    {
        if (blackjack) return 1;
        if (soloAce) return (decimal) 4 / 13;
        if (soloTen) return (decimal) 1 / 13;
        return 0;
    }

    private Hand Hit(int hit)
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