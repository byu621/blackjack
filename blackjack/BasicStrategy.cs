namespace blackjack;

public class BasicStrategy
{
    private readonly Dictionary<State, Action> _dict = new();

    private readonly Dictionary<string, Action> _actionDict = new()
    {
        { "H", Action.Hit },
        { "S", Action.Stand },
        { "D", Action.Double },
        { "Sp", Action.Split },
    };
    
    public BasicStrategy()
    {
        Load("HardBasicStrategy.csv", Shape.Hard);
        Load("SoftBasicStrategy.csv", Shape.Soft);
    }

    public Action GetAction(State state)
    {
        if (!_dict.TryGetValue(state, out Action value))
        {
            throw new Exception("Invalid state");
        }

        return value;
    }
    
    private void Load(string fileName, Shape shape)
    {
        string[] lines = File.ReadAllLines(fileName);
        int[] dealerValues = lines[0][2..].Split(",").Select(x => x == "A" ? 1 : int.Parse(x)).ToArray();
        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            string[] split = line.Split(",");
            int value = int.Parse(split[0]);

            for (int j = 1; j < split.Length; j++)
            {
                int dealer = dealerValues[j - 1];
                State state = new(shape, value, dealer);
                Action action = _actionDict[split[j]];
                _dict[state] = action;
            }
        }
    }
}