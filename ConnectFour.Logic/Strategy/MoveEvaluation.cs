namespace ConnectFour.Logic.Strategy
{
    public static class MoveEvaluation
    {
        private static readonly int[,] evaluationTable = {{3, 4, 5, 7, 5, 4, 3}, 
                                          {4, 6, 8, 10, 8, 6, 4},
                                          {5, 8, 11, 13, 11, 8, 5}, 
                                          {5, 8, 11, 13, 11, 8, 5},
                                          {4, 6, 8, 10, 8, 6, 4},
                                          {3, 4, 5, 7, 5, 4, 3}};

        //here is where the evaluation table is called
        public static double evaluateContent(int[,] gamefield, int player)
        {
            int utility = 128;
            int sum = 0;
            for (int y = 0; y < 6; y++)
                for (int x = 0; x < 7; x++)
                    if (gamefield[x, y] == player)
                        sum += evaluationTable[y, x];
                    else if (gamefield[x, y] == 0)
                        sum += 0;
                    else
                        sum -= evaluationTable[y, x];

            int value = utility + sum;

            // normalisieren
            // 255 entspricht 2, danach wird noch eins abgezeogen, da im restlichen Programm die -1 als verloren gilt
            double returnValue = (2.0/255.0)*value;

            returnValue -= 1;

            return returnValue;
        }
    }
}
