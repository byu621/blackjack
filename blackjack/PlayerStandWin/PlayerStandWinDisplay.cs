using CsvHelper.Configuration;

namespace blackjack;
public struct PlayerStandWinDisplay { 
    public int PlayerTotal { get; set; }
    public Probability DealerA { get; set; }
    public Probability Dealer10 { get; set; }
    public Probability Dealer9 { get; set; }
    public Probability Dealer8 { get; set; }
    public Probability Dealer7 { get; set; }
    public Probability Dealer6 { get; set; }
    public Probability Dealer5 { get; set; }
    public Probability Dealer4 { get; set; }
    public Probability Dealer3 { get; set; }
    public Probability Dealer2 { get; set; }

}

public sealed class PlayerStandWinDisplayMap : ClassMap<PlayerStandWinDisplay>
{
    public PlayerStandWinDisplayMap()
    {
        Map(m => m.PlayerTotal).Name("#");
        Map(m => m.DealerA).Name("A");
        Map(m => m.Dealer10).Name("10");
        Map(m => m.Dealer9).Name("9");
        Map(m => m.Dealer8).Name("8"); 
        Map(m => m.Dealer7).Name("7"); 
        Map(m => m.Dealer6).Name("6"); 
        Map(m => m.Dealer5).Name("5"); 
        Map(m => m.Dealer4).Name("4"); 
        Map(m => m.Dealer3).Name("3"); 
        Map(m => m.Dealer2).Name("2"); 
    }
}

    