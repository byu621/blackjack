using CsvHelper.Configuration;

namespace blackjack;

public readonly struct Hand
{
    public HandType HandType { get; }
    public int Total { get; }

    public Hand(HandType handType, int total)
    {
        //if (total < 2 || total > 22)
        //{
            //throw new ArgumentException($"Hand is not valid handType = {handType} total = {total}");
        //}

        HandType = handType;
        Total = total;
    }
}