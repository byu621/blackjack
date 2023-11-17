using CsvHelper.Configuration;

namespace blackjack;
public struct PlayerStandEvDisplay { 
    public Hand PlayerHand { get; set; }
    public Ev DealerA { get; set; }
    public Ev Dealer10 { get; set; }
    public Ev Dealer9 { get; set; }
    public Ev Dealer8 { get; set; }
    public Ev Dealer7 { get; set; }
    public Ev Dealer6 { get; set; }
    public Ev Dealer5 { get; set; }
    public Ev Dealer4 { get; set; }
    public Ev Dealer3 { get; set; }
    public Ev Dealer2 { get; set; }

}

public sealed class PlayerStandEvDisplayMap : ClassMap<PlayerStandEvDisplay>
{
    public PlayerStandEvDisplayMap()
    {
        Map(m => m.PlayerHand).Name("#").Convert(args => args.Value.PlayerHand.HandType == HandType.BLACKJACK ? "BJ" : args.Value.PlayerHand.Total.ToString());
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

    