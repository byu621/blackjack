using blackjack.Structs;
using static blackjack.Structs.Probability;

namespace blackjack;

/**
 * Each hard hand total 4..21 -> P(17), P(18), P(19), P(20), P(21), P(B)
 */
public class Table
{
    private readonly Dictionary<int, Probability> _totalProbabilities = new()
    {
        {1, OneThirteenth },
        {2, OneThirteenth },
        {3, OneThirteenth },
        {4, OneThirteenth },
        {5, OneThirteenth },
        {6, OneThirteenth },
        {7, OneThirteenth },
        {8, OneThirteenth },
        {9, OneThirteenth },
        {10, OneThirteenth }
    };
    private readonly Dictionary<int, DealerStayTotalProbability> _dictionary = new();
    
    public Table()
    {
    }

    public void Compute()
    {
        _dictionary.Add(21, new(Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(20, new(Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(19, new(Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(18, new(Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(17, new(Zero, Zero, Zero, Zero, One, Zero));
        
        // possible increases to a hand's total is 1..11

        for (int i = 1; i <= 11; i++)
        {
            Probability p17 = _dictionary[16 + 1].P17 * OneThirteenth;
            Console.WriteLine(p17);
        }
        
    }
}