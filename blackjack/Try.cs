namespace blackjack;

public class Try 
{
    private const int heightHard = 20;
    private const int heightSoft = 11;
    private decimal[][] dataHard = new decimal[heightHard][];
    private decimal[][] dataSoft = new decimal[heightSoft][];

    // P17, P18, P19, P20, P21, PBust
    private const int width = 6;

    public void Compute() 
    {
        dataHard[0] = new decimal[width] {0, 0, 0, 0, 1, 0};
        dataHard[1] = new decimal[width] {0, 0, 0, 1, 0, 0};
        dataHard[2] = new decimal[width] {0, 0, 1, 0, 0, 0};
        dataHard[3] = new decimal[width] {0, 1, 0, 0, 0, 0};
        dataHard[4] = new decimal[width] {1, 0, 0, 0, 0, 0};

        dataSoft[0] = new decimal[width] {0, 0, 0, 0, 1, 0}; // 21
        dataSoft[1] = new decimal[width] {0, 0, 0, 1, 0, 0}; // 20
        dataSoft[2] = new decimal[width] {0, 0, 1, 0, 0, 0}; // 19
        dataSoft[3] = new decimal[width] {0, 1, 0, 0, 0, 0}; // 18
        dataSoft[4] = new decimal[width] {1, 0, 0, 0, 0, 0}; // 17
        ComputeHard(heightHard - 1); // 2
        ComputeHard(heightHard - 2); // 3
        ComputeSoft(heightSoft - 1); // A
    }

    private decimal[] ComputeHard(int i)
    {
        if (i < 0) return new decimal[width] {0, 0, 0, 0, 0, 1};

        if (dataHard[i] != null)
        {
            return dataHard[i];
        }

        decimal[] result = new decimal[6];
        for (int hitCard = 1; hitCard <= 10; hitCard++)
        {
            decimal probability = hitCard == 10 ? (decimal) 4 / 13 : (decimal) 1 / 13;
            for (int col = 0; col < width; col++)
            {
                if (hitCard == 1)
                {
                    int hardValue = 21 - i;
                    int softValue = hardValue + 11;
                    int softIndex = 21 - softValue;
                    result[col] += ComputeSoft(softIndex)[col] * probability;
                }
                else 
                {
                    result[col] += ComputeHard(i - hitCard)[col] * probability;
                }
            }                
        }

        dataHard[i] = result;
        return result;
    }

    private decimal[] ComputeSoft(int i)
    {
        if (i < 0)
        {
            int softValue = 21 - i;
            int hardValue = softValue - 10;
            int hardIndex = 21 - hardValue;
            return ComputeHard(hardIndex);
        }

        if (dataSoft[i] != null)
        {
            return dataSoft[i];
        }

        decimal[] result = new decimal[6];
        for (int hitCard = 1; hitCard <= 10; hitCard++)
        {
            decimal probability = hitCard == 10 ? (decimal) 4 / 13 : (decimal) 1 / 13;
            for (int col = 0; col < width; col++)
            {
                result[col] += ComputeSoft(i - hitCard)[col] * probability;
            }
        }

        dataSoft[i] = result;
        return result;
    }

    public void PrintToConsole()
    {
        for (int i = 0; i < heightHard; i++)
        {
            for (int j = 0 ; j < width; j++)
            {
                Console.Write((dataHard[i][j] * 100).ToString("0.00"));
                Console.Write(",");
            }
            Console.WriteLine();
        }

        for (int i = 0; i < heightSoft; i++)
        {
            for (int j = 0 ; j < width; j++)
            {
                Console.Write((dataSoft[i][j] * 100).ToString("0.00"));
                Console.Write(",");
            }
            Console.WriteLine();
        }
    }
}