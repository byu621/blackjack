namespace blackjack;

public record Hand 
{
    public Shape Shape { get; }
    public int Value { get; }
    private readonly bool soloTen;
    private readonly bool soloAce;
    public bool Blackjack { get; }

    private static readonly Dictionary<(Hand, Hand), decimal> PlayerStandCache = new();
    private static readonly Dictionary<(Hand, Hand), decimal> PlayerHitCache = new();

    public Hand(Shape shape, int value, bool soloAce, bool soloTen, bool blackjack)
    {
        if (soloAce && (shape != Shape.SOFT || value != 11)) throw new ArgumentException("ERROR");
        if (soloTen && (shape != Shape.HARD || value != 10)) throw new ArgumentException("ERROR");
        if (blackjack && (shape != Shape.SOFT || value != 21)) throw new ArgumentException("ERROR");

        Shape = shape;
        Value = value;
        this.soloAce = soloAce;
        this.soloTen = soloTen;
        Blackjack = blackjack;
    }

    public decimal CalculateDealerBust()
    {
        if (Value > 21 && Shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, Value - 10, false, false, false);
            return hand.CalculateDealerBust();
        }

        if (Value > 21) return 1;
        if (Value >= 17) return 0;
        
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

        if (Value > 21 && Shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, Value - 10, false, false, false);
            return hand.CalculateDTP(target);
        }

        if (Value > 21) return 0;
        if (Value == target) return 1;
        if (Value > target) return 0;
        if (Value >= 17) return 0;
        
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
        if (Value > 21)
        {
            return -1;
        }
        
        if (PlayerStandCache.ContainsKey((this, dealer)))
        {
            return PlayerStandCache[(this, dealer)];
        }

        decimal ev;
        if (Blackjack)
        {
            decimal push = dealer.ProbabilityBlackjack();
            decimal win = 1 - push;
            decimal lose = 0;
            ev = (win - lose) * 1.5m;
        }
        else if (Value == 21)
        {
            decimal push = dealer.CalculateDTP(Value) - dealer.ProbabilityBlackjack();
            decimal win = dealer.CalculateDealerBust() + dealer.CalculateDTPBelowTarget(Value);
            decimal lose = 1 - push - win;
            ev = win - lose;
        }
        else 
        {
            decimal push = dealer.CalculateDTP(Value);
            decimal win = dealer.CalculateDealerBust() + dealer.CalculateDTPBelowTarget(Value);
            decimal lose = 1 - push - win;
            ev = win - lose;
        }

        PlayerStandCache[(this, dealer)] = ev;
        return ev;
    }
    
    public decimal CalculatePHEV(Hand dealer)
    {
        if (PlayerHitCache.ContainsKey((this, dealer)))
        {
            return PlayerHitCache[(this, dealer)];
        }
        
        if (Value > 21 && Shape == Shape.SOFT) 
        {
            Hand hand = new Hand(Shape.HARD, Value - 10, false, false, false);
            return hand.CalculatePHEV(dealer);
        }

        if (Value > 21)
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
        decimal max = Math.Max(standEv, hitEv);
        PlayerHitCache[(this, dealer)] = max;
        return max;
    }

    public decimal CalculatePDEV(Hand dealer)
    {
        if (Value > 21)
        {
            throw new ArgumentException("ERROR");
        }
        
        decimal ev = 0;
        for (int hit = 1; hit <= 10; hit++)
        {
            decimal frequency = hit == 10 ? 4m : 1m;
            Hand hand = Hit(hit);
            decimal standEv = hand.CalculatePSEV(dealer);
            ev += standEv * frequency;
        }

        ev /= 13;
        ev *= 2;
        return ev;
    }

    public Action CalculatePHSAction(Hand dealer)
    {
        decimal standEv = CalculatePSEV(dealer);
        decimal hitEv = CalculatePHEV(dealer);
        return standEv >= hitEv ? Action.S : Action.H;
    }
    
    private decimal ProbabilityBlackjack()
    {
        if (Blackjack) return 1;
        if (soloAce) return (decimal) 4 / 13;
        if (soloTen) return (decimal) 1 / 13;
        return 0;
    }

    private Hand Hit(int hit)
    {
        int hitA = hit == 1 && Shape == Shape.HARD && Value < 11 ? 11 : hit;
        int newValue = Value + hitA;
        
        Shape newShape = Shape == Shape.SOFT || hitA == 11 ? Shape.SOFT : Shape.HARD;

        if (newValue > 21 && newShape == Shape.SOFT)
        {
            newValue -= 10;
            newShape = Shape.HARD;
        }
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