using Data;

namespace MCTG.Models
{
    internal record DeckEntry : DbRecord
    {
        public Guid Owner { get; set; }
        public Guid Card { get; set; }

        public DeckEntry() { }

        public DeckEntry(Guid user, Guid card)
        {
            Owner = user;
            Card = card;
        }
    }
}
