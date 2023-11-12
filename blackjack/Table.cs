using blackjack.Structs;
using static blackjack.Structs.Probability;
using static blackjack.Structs.DealerStayTotalProbability;

namespace blackjack;

/**
 * Each hard hand total 4..21 -> P(17), P(18), P(19), P(20), P(21), P(B)
 */
public class Table
{
    private readonly Dictionary<int, DealerStayTotalProbability> _dictionary = new();
    private List<string> _headers = new List<string>()
    {
        "#", "17", "18", "19", "20", "21", "B"
    };

    public Table()
    {
    }

    public void Compute()
    {
        _dictionary.Add(21, new(Zero, Zero, Zero, Zero, One, Zero));
        _dictionary.Add(20, new(Zero, Zero, Zero, One, Zero, Zero));
        _dictionary.Add(19, new(Zero, Zero, One, Zero, Zero, Zero));
        _dictionary.Add(18, new(Zero, One, Zero, Zero, Zero, Zero));
        _dictionary.Add(17, new(One, Zero, Zero, Zero, Zero, Zero));

        // possible increases to a hand's total is 1..10
        for (int startingHand = 16; startingHand >= 2;  startingHand--)
        {
            Probability p17 = Zero;
            Probability p18 = Zero;
            Probability p19 = Zero;
            Probability p20 = Zero;
            Probability p21 = Zero;
            Probability pBust = Zero;
            for (int hitCard = 1; hitCard <= 10; hitCard++)
            {
                int handTotal = startingHand + hitCard;
                Probability handTotalProbability = hitCard == 10 ? FourThirteenth : OneThirteenth;

                p17 += _dictionary.GetValueOrDefault(handTotal, OVER_21).P17 * handTotalProbability;
                p18 += _dictionary.GetValueOrDefault(handTotal, OVER_21).P18 * handTotalProbability;
                p19 += _dictionary.GetValueOrDefault(handTotal, OVER_21).P19 * handTotalProbability;
                p20 += _dictionary.GetValueOrDefault(handTotal, OVER_21).P20 * handTotalProbability;
                p21 += _dictionary.GetValueOrDefault(handTotal, OVER_21).P21 * handTotalProbability;
                pBust += _dictionary.GetValueOrDefault(handTotal, OVER_21).PBust * handTotalProbability;
            }

            _dictionary.Add(startingHand, new(p17, p18, p19, p20, p21, pBust));

            Console.WriteLine($"{startingHand}: {_dictionary[startingHand]}");
        }
    }

    public string ToMarkdown()
    {
        string markdown = string.Empty;
        markdown += "|";
        foreach(string header in _headers)
        {
            markdown += $"{header}|";
        }
        markdown += "\n";
        markdown += "|-|-|-|-|-|-|-|\n";

        for (int total = 21; total >= 2; total--)
        {
            markdown += $"|{total}|{_dictionary[total].P17}|{_dictionary[total].P18}|{_dictionary[total].P19}|{_dictionary[total].P20}|{_dictionary[total].P21}|{_dictionary[total].PBust}\n";
        }

        return markdown;
    }
}