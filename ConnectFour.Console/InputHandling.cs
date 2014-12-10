using System;
using System.Drawing;
using Newtonsoft.Json;

namespace ConnectFour.Console
{
    public static class InputHandling
    {
        public static ProcessedInformation ProcessInput(int[,] oldGameField, int[,] newGameField)
        {
            if (!checkFormat(oldGameField) && !checkFormat(newGameField))
                return new ProcessedInformation(InputState.FORMATERROR, new Point(-1, -1));

            int diffCount = 0;
            InputState inputState = InputState.UNDEFINED;
            Point playedMove = new Point(-1, -1);

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (oldGameField[x, y] != newGameField[x, y])
                    {
                        diffCount++;
                        playedMove = new Point(x, y);
                        if(newGameField[x,y] == 1)
                            inputState = InputState.PLAYER1;
                        if(newGameField[x,y] == 2)
                            inputState = InputState.PLAYER2;
                    }
                }
            }

            if(diffCount > 1 || playedMove.Y != 5 && playedMove.X > 0 && oldGameField[playedMove.X, playedMove.Y + 1] == 0)
                return new ProcessedInformation(InputState.CHEAT, new Point(-1, -1));

            if (diffCount == 0)
                inputState = InputState.NOCHANGE;

            return new ProcessedInformation(inputState, playedMove);
        }

        public static int[,] Get2DArrayFromJSON(string jSonString)
        {
            try
            {
                int[,] deserializeObject = JsonConvert.DeserializeObject<int[,]>(jSonString);
                return deserializeObject;
            }
            catch (Exception)
            {
                return new int[0, 0];
            }
        }

        public static string GetJsonFrom2DArray(int[,] array)
        {
            string serializedObject = JsonConvert.SerializeObject(array);
            return serializedObject;
        }

        private static bool checkFormat(int[,] gameField)
        {
            return gameField.GetLength(0) == 7 && gameField.GetLength(1) == 6;
        }
    }
}
