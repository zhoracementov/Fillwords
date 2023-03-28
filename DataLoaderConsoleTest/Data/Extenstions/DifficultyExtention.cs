namespace DataLoaderConsoleTest.Data.Extenstions
{
    internal static class DifficultyExtention
    {
        public static double GetFactor(this Difficulty difficulty)
        {
            return (int)difficulty / (int)Difficulty.Hard;
        }

        public static double GetFactorInverse(this Difficulty difficulty)
        {
            return (int)Difficulty.Hard / (int)difficulty;
        }
    }
}
