namespace DataLoaderConsoleTest.Table
{
    internal static class DifficultyExtention
    {
        public static double GetFactor(this FillwordDifficulty difficulty)
        {
            return (double)(int)difficulty / (int)FillwordDifficulty.Hard;
        }

        public static double GetFactorInverse(this FillwordDifficulty difficulty)
        {
            return (double)(int)FillwordDifficulty.Hard / (int)difficulty;
        }
    }
}
