namespace FillwordWPF.Models
{
    internal class Game
    {
        public Fillword Fillword { get; }

        public Game()
        {
            Fillword = new FillwordGenerator().Fillword;
        }
    }
}
